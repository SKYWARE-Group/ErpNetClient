using Flurl;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Skyware.ErpNetFS.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Skyware.ErpNetFS;

// Ignore Spelling: taskinfo rawrequest reversalreceipt erp zreport xreport

/// <summary>
/// Client for interacting with the ErpNet.FS fiscal service.
/// Provides methods for communicating with fiscal printers through the ErpNet.FS server.
/// This implementation supports parallel invocations across multiple threads.
/// </summary>
public class Client : IDisposable
{
    // The HttpClient instance is shared across all requests to optimize performance.
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private bool _disposed;

    private static readonly JsonSerializerOptions serializeOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    /// <summary>
    /// Base URL of the ErpNet.FS server.
    /// </summary>
    public string BaseUrl { get; }

    /// <summary>
    /// Default printer device ID to use for operations.
    /// If not specified in methods, this value will be used.
    /// </summary>
    public string DeviceId { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the ErpNet.FS client.
    /// </summary>
    /// <param name="baseUrl">Base URL of the ErpNet.FS server (default: http://localhost:8001/)</param>
    /// <param name="logger">Logger for diagnostic information (optional)</param>
    /// <exception cref="ArgumentNullException">Thrown when baseUrl is null</exception>
    public Client(string baseUrl = "http://localhost:8001/", ILogger logger = null)
    {
        BaseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
        _logger = logger ?? NullLogger.Instance;

        // Create a single HttpClient instance with optimal connection settings
        // Configure max connections for better parallel processing
        HttpClientHandler handler = new() { MaxConnectionsPerServer = 20 };

        _httpClient = new HttpClient(handler, disposeHandler: true);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    /// <summary>
    /// Retrieves list of all configured printers in the ErpNet.FS service.
    /// </summary>
    /// <returns>Dictionary of printer IDs and their corresponding device information</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the client has been disposed</exception>
    public Task<Dictionary<string, DeviceInfo>> GetPrintersAsync()
    {
        ThrowIfDisposed();
        return SendGetRequestAsync<Dictionary<string, DeviceInfo>>(BuildUrl("printers"));
    }

    /// <summary>
    /// Retrieves detailed information about a specific printer including model, URI, and configuration.
    /// </summary>
    /// <param name="deviceId">ID of the printer (optional, uses default DeviceId if not specified)</param>
    /// <returns>Detailed information about the specified printer</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the client has been disposed</exception>
    public Task<DeviceInfo> GetPrinterInfoAsync(string deviceId = null)
    {
        ThrowIfDisposed();
        return SendGetRequestAsync<DeviceInfo>(BuildUrl("printers", ResolveDeviceId(deviceId)));
    }

    /// <summary>
    /// Retrieves the current status of a printer including error states and operational information.
    /// </summary>
    /// <param name="deviceId">ID of the printer (optional, uses default DeviceId if not specified)</param>
    /// <returns>Current status of the printer with timestamp</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the client has been disposed</exception>
    public Task<DeviceStatusWithDateTime> GetPrinterStatusAsync(string deviceId = null)
    {
        ThrowIfDisposed();
        return SendGetRequestAsync<DeviceStatusWithDateTime>(BuildUrl("printers", ResolveDeviceId(deviceId), "status"));
    }

    /// <summary>
    /// Sends raw commands directly to the printer for low-level operations.
    /// </summary>
    /// <param name="request">Raw request containing commands to be sent to the printer</param>
    /// <param name="deviceId">ID of the printer (optional, uses default DeviceId if not specified)</param>
    /// <returns>Status and raw response from the printer</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the client has been disposed</exception>
    public Task<DeviceStatusWithRawResponse> SendRawRequestAsync(RawRequest request, string deviceId = null)
    {
        ThrowIfDisposed();
        return SendPostRequestAsync<RawRequest, DeviceStatusWithRawResponse>(
            BuildUrl("printers", ResolveDeviceId(deviceId), "rawrequest"),
            request);
    }

    /// <summary>
    /// Registers a sale by printing a fiscal receipt on the specified printer.
    /// </summary>
    /// <param name="receipt">Receipt data containing items, payment types, and other fiscal information</param>
    /// <param name="deviceId">ID of the printer (optional, uses default DeviceId if not specified)</param>
    /// <returns>Status and receipt information including fiscal receipt number</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the client has been disposed</exception>
    public Task<DeviceStatusWithReceiptInfo> PrintFiscalReceiptAsync(Receipt receipt, string deviceId = null)
    {
        ThrowIfDisposed();
        return SendPostRequestAsync<Receipt, DeviceStatusWithReceiptInfo>(
            BuildUrl("printers", ResolveDeviceId(deviceId), "receipt"),
            receipt);
    }

    /// <summary>
    /// Registers a refund by printing a reversal (refund) receipt on the specified printer.
    /// </summary>
    /// <param name="receipt">Reversal receipt data with refund information</param>
    /// <param name="deviceId">ID of the printer (optional, uses default DeviceId if not specified)</param>
    /// <returns>Status and receipt information including fiscal receipt number</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the client has been disposed</exception>
    public Task<DeviceStatusWithReceiptInfo> PrintRefundReceiptAsync(ReversalReceipt receipt, string deviceId = null)
    {
        ThrowIfDisposed();
        return SendPostRequestAsync<ReversalReceipt, DeviceStatusWithReceiptInfo>(
            BuildUrl("printers", ResolveDeviceId(deviceId), "reversalreceipt"),
            receipt);
    }

    /// <summary>
    /// Prints a Z Report, which ends the sales day and can be used for bookkeeping.
    /// Z Reports close the fiscal period and reset daily totals.
    /// </summary>
    /// <param name="deviceId">ID of the printer (optional, uses default DeviceId if not specified)</param>
    /// <returns>Status with date and time information</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the client has been disposed</exception>
    public Task<DeviceStatusWithDateTime> PrintZReportAsync(string deviceId = null)
    {
        ThrowIfDisposed();
        return PrintReportAsync(true, deviceId);
    }

    /// <summary>
    /// Prints an X Report, which provides a summary of sales without closing the fiscal day.
    /// X Reports provide information for the current period without resetting totals.
    /// </summary>
    /// <param name="deviceId">ID of the printer (optional, uses default DeviceId if not specified)</param>
    /// <returns>Status with date and time information</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the client has been disposed</exception>
    public Task<DeviceStatusWithDateTime> PrintXReportAsync(string deviceId = null)
    {
        ThrowIfDisposed();
        return PrintReportAsync(false, deviceId);
    }

    /// <summary>
    /// Prints a duplicate of the last printed fiscal receipt.
    /// </summary>
    /// <param name="deviceId">ID of the printer (optional, uses default DeviceId if not specified)</param>
    /// <returns>Status of the printer after printing duplicate</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the client has been disposed</exception>
    public Task<DeviceStatus> PrintDuplicateAsync(string deviceId = null)
    {
        ThrowIfDisposed();
        return SendPostRequestAsync<object, DeviceStatus>(
            BuildUrl("printers", ResolveDeviceId(deviceId), "duplicate"),
            null);
    }

    /// <summary>
    /// Retrieves the current cash balance of the fiscal device.
    /// </summary>
    /// <param name="deviceId">ID of the printer (optional, uses default DeviceId if not specified)</param>
    /// <returns>Status with cash amount information</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the client has been disposed</exception>
    public Task<DeviceStatusWithCashAmount> GetBalanceAsync(string deviceId = null)
    {
        ThrowIfDisposed();
        return SendGetRequestAsync<DeviceStatusWithCashAmount>(
            BuildUrl("printers", ResolveDeviceId(deviceId), "cash"));
    }

    /// <summary>
    /// Registers a deposit of money to the cash register and prints a deposit slip.
    /// </summary>
    /// <param name="deposit">Amount to deposit and optional description</param>
    /// <param name="deviceId">ID of the printer (optional, uses default DeviceId if not specified)</param>
    /// <returns>Status of the printer after deposit operation</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the client has been disposed</exception>
    public Task<DeviceStatus> DepositMoneyAsync(DepositWithdraw deposit, string deviceId = null)
    {
        ThrowIfDisposed();
        return SendPostRequestAsync<DepositWithdraw, DeviceStatus>(
            BuildUrl("printers", ResolveDeviceId(deviceId), "deposit"),
            deposit);
    }

    /// <summary>
    /// Registers a withdrawal of money from the cash register and prints a withdrawal slip.
    /// </summary>
    /// <param name="withdraw">Amount to withdraw and optional description</param>
    /// <param name="deviceId">ID of the printer (optional, uses default DeviceId if not specified)</param>
    /// <returns>Status of the printer after withdrawal operation</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the client has been disposed</exception>
    public Task<DeviceStatus> WithdrawMoneyAsync(DepositWithdraw withdraw, string deviceId = null)
    {
        ThrowIfDisposed();
        return SendPostRequestAsync<DepositWithdraw, DeviceStatus>(
            BuildUrl("printers", ResolveDeviceId(deviceId), "withdraw"),
            withdraw);
    }

    /// <summary>
    /// Synchronizes the fiscal device date and time with the provided time.
    /// </summary>
    /// <param name="currentTime">Date and time to set on the fiscal device</param>
    /// <param name="deviceId">ID of the printer (optional, uses default DeviceId if not specified)</param>
    /// <returns>Status of the printer after time synchronization</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the client has been disposed</exception>
    public Task<DeviceStatus> SyncPrinterTimeAsync(CurrentDateTime currentTime, string deviceId = null)
    {
        ThrowIfDisposed();
        return SendPostRequestAsync<CurrentDateTime, DeviceStatus>(
            BuildUrl("printers", ResolveDeviceId(deviceId), "datetime"),
            currentTime);
    }

    /// <summary>
    /// Prints a specific report type (X or Z report).
    /// </summary>
    /// <param name="deviceId">ID of the printer</param>
    /// <param name="action">Report type to print (xreport or zreport)</param>
    /// <returns>Status with date and time information</returns>
    /// <exception cref="ObjectDisposedException">Thrown if the client has been disposed</exception>
    public Task<DeviceStatusWithDateTime> PrintReportAsync(string deviceId, string action)
    {
        ThrowIfDisposed();
        return SendPostRequestAsync<object, DeviceStatusWithDateTime>(
            BuildUrl("printers", ResolveDeviceId(deviceId), action),
            null);
    }

    /// <summary>
    /// Base function for printing fiscal reports (X or Z reports).
    /// </summary>
    /// <param name="closeDay">If true, prints Z report (closes day); if false, prints X report</param>
    /// <param name="deviceId">ID of the printer (optional, uses default DeviceId if not specified)</param>
    /// <returns>Status with date and time information</returns>
    private Task<DeviceStatusWithDateTime> PrintReportAsync(bool closeDay, string deviceId = null)
    {
        string reportType = closeDay ? "zreport" : "xreport";
        return SendPostRequestAsync<object, DeviceStatusWithDateTime>(
            BuildUrl("printers", ResolveDeviceId(deviceId), reportType),
            null);
    }

    /// <summary>
    /// Resolves the device ID to use, using the provided ID or falling back to the default.
    /// </summary>
    /// <param name="deviceId">Optional device ID</param>
    /// <returns>Resolved device ID to use</returns>
    private string ResolveDeviceId(string deviceId) =>
        string.IsNullOrWhiteSpace(deviceId) ? DeviceId : deviceId;

    /// <summary>
    /// Builds a URL by appending path segments to the base URL.
    /// </summary>
    /// <param name="segments">Path segments to append</param>
    /// <returns>Complete URL as string</returns>
    private string BuildUrl(params string[] segments)
    {
        var url = BaseUrl;
        foreach (var segment in segments)
            url = url.AppendPathSegment(segment);
        return url.ToString();
    }

    /// <summary>
    /// Throws an ObjectDisposedException if this instance has been disposed.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown if the client has been disposed</exception>
    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(Client), "This client has been disposed and cannot be used anymore.");
        }
    }

    /// <summary>
    /// Sends an HTTP GET request and processes the response.
    /// </summary>
    /// <typeparam name="TResponse">Type to deserialize the response to</typeparam>
    /// <param name="url">URL to send the request to</param>
    /// <returns>Deserialized response object</returns>
    /// <exception cref="HttpRequestException">Thrown when the request fails or the response status code is not successful (2xx)</exception>
    /// <exception cref="ObjectDisposedException">Thrown if the client has been disposed</exception>
    private async Task<TResponse> SendGetRequestAsync<TResponse>(string url)
    {
        ThrowIfDisposed();
        _logger.LogDebug("HTTP GET Request: {Url}", url);

        // Use the shared HttpClient instance directly
        HttpResponseMessage res = await _httpClient.GetAsync(url);
        string resStr = await res.Content.ReadAsStringAsync();

        _logger.LogDebug("HTTP GET Response from {Url} (Status: {StatusCode}): {Response}",
            url, res.StatusCode, resStr);

        if (!res.IsSuccessStatusCode)
        {
            _logger.LogError("HTTP GET Request to {Url} failed with status code {StatusCode}: {Response}",
                url, res.StatusCode, resStr);
            throw new HttpRequestException($"Request to {url} failed with status code {res.StatusCode}: {resStr}");
        }

        return JsonSerializer.Deserialize<TResponse>(resStr, serializeOptions);
    }

    /// <summary>
    /// Sends an HTTP POST request with JSON content and processes the response.
    /// </summary>
    /// <typeparam name="TRequest">Type of the request object to serialize</typeparam>
    /// <typeparam name="TResponse">Type to deserialize the response to</typeparam>
    /// <param name="url">URL to send the request to</param>
    /// <param name="request">Request object to serialize</param>
    /// <returns>Deserialized response object</returns>
    /// <exception cref="HttpRequestException">Thrown when the request fails or the response status code is not successful (2xx)</exception>
    /// <exception cref="ObjectDisposedException">Thrown if the client has been disposed</exception>
    private async Task<TResponse> SendPostRequestAsync<TRequest, TResponse>(string url, TRequest request)
    {
        ThrowIfDisposed();

        StringContent content;
        string requestJson = string.Empty;

        if (request is null)
        {
            content = new(string.Empty, Encoding.UTF8, "application/json");
            _logger.LogDebug("HTTP POST Request to {Url}: <empty body>", url);
        }
        else
        {
            requestJson = JsonSerializer.Serialize(request, serializeOptions);
            content = new(requestJson, Encoding.UTF8, "application/json");
            _logger.LogDebug("HTTP POST Request to {Url}: {RequestBody}", url, requestJson);
        }

        // Use the shared HttpClient instance directly
        HttpResponseMessage res = await _httpClient.PostAsync(url, content);
        string resStr = await res.Content.ReadAsStringAsync();

        _logger.LogDebug("HTTP POST Response from {Url} (Status: {StatusCode}): {Response}",
            url, res.StatusCode, resStr);

        if (!res.IsSuccessStatusCode)
        {
            _logger.LogError("HTTP POST Request to {Url} failed with status code {StatusCode}. Request: {RequestBody}, Response: {Response}",
                url, res.StatusCode, requestJson, resStr);
            throw new HttpRequestException($"Request to {url} failed with status code {res.StatusCode}: {resStr}");
        }

        return JsonSerializer.Deserialize<TResponse>(resStr, serializeOptions);
    }

    /// <summary>
    /// Disposes the resources used by this client.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the resources used by this client.
    /// </summary>
    /// <param name="disposing">Whether this method is being called from Dispose() or a finalizer</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Dispose of managed resources
                _httpClient.Dispose();
            }

            _disposed = true;
        }
    }
}
