using Refit;
using Skyware.ErpNetFS.Model;

namespace ErpNetModelDemo;

// Ignore Spelling: rawrequest reversalreceipt xreport zreport tremol

/// <summary>
/// Interface for the ErpNet.FS client.
/// </summary>
public interface IFiscalPrinterClient
{

    /// <summary>
    /// Retrieves server variables.
    /// </summary>
    /// <returns>Server variables (<see cref="ServerVariables"/>)</returns>
    [Get("/service/vars")]
    Task<ServerVariables> GetServerVariablesAsync();

    /// <summary>
    /// Retrieves list of configured printers.
    /// </summary>
    /// <returns>List of all configured printers</returns>
    [Get("/printers")]
    Task<Dictionary<string, DeviceInfo>> GetPrintersAsync();

    /// <summary>
    /// Retrieves printer information - model, uri, etc.
    /// </summary>
    /// <param name="deviceId">Id of the printer</param>
    /// <returns>Information for a given device</returns>
    [Get("/printers/{deviceId}")]
    Task<DeviceInfo> GetPrinterAsync(string deviceId);

    /// <summary>
    /// Retrieves printer status.
    /// </summary>
    /// <param name="deviceId">Id of the printer</param>
    /// <returns>Device status</returns>
    [Get("/printers/{deviceId}/status")]
    Task<DeviceStatusWithDateTime> GetPrinterStatusAsync(string deviceId);

    /// <summary>
    /// Register sale and prints fiscal receipt.
    /// </summary>
    /// <param name="deviceId">Id of the printer</param>
    /// <param name="receipt">Fiscal <see cref="Receipt"/> to be registered</param>
    /// <returns>Receipt status</returns>
    [Post("/printers/{deviceId}/receipt")]
    Task<DeviceStatusWithReceiptInfo> PrintFiscalReceiptAsync(string deviceId, Receipt receipt);

    /// <summary>
    /// Prints duplicate of the last printed fiscal receipt.
    /// </summary>
    /// <param name="deviceId">Id of the printer</param>
    /// <returns></returns>
    [Post("/printers/{deviceId}/duplicate")]
    Task<DeviceStatus> PrintDuplicateAsync(string deviceId);

    /// <summary>
    /// Register refund and prints reversal fiscal receipt.
    /// </summary>
    /// <param name="deviceId">Id of the printer</param>
    /// <param name="receipt">Refund (<see cref="ReversalReceipt"/>) to be registered</param>
    /// <returns>Receipt status</returns>
    [Post("/printers/{deviceId}/reversalreceipt")]
    Task<DeviceStatusWithReceiptInfo> PrintRefundReceiptAsync(string deviceId, ReversalReceipt receipt);

    /// <summary>
    /// Retrieves the balance (cash) of the cash register.
    /// </summary>
    /// <param name="deviceId">Id of the printer</param>
    /// <returns></returns>
    [Get("/printers/{deviceId}/cash")]
    Task<DeviceStatusWithCashAmount> GetBalanceAsync(string deviceId);

    /// <summary>
    /// Registers deposit money to the cash register and prints a slip.
    /// </summary>
    /// <param name="deviceId">Id of the printer</param>
    /// <param name="deposit">Amount of deposit (<see cref="DepositWithdraw"/>)</param>
    /// <returns></returns>
    [Post("/printers/{deviceId}/deposit")]
    Task<DeviceStatus> DepositMoneyAsync(string deviceId, DepositWithdraw deposit);

    /// <summary>
    /// Registers withdraw money from the cash register and prints a slip.
    /// </summary>
    /// <param name="deviceId">Id of the printer</param>
    /// <param name="withdraw">Amount of withdraw (<see cref="DepositWithdraw"/>)</param>
    /// <returns></returns>
    [Post("/printers/{deviceId}/withdraw")]
    Task<DeviceStatus> WithdrawMoneyAsync(string deviceId, DepositWithdraw withdraw);

    /// <summary>
    /// Sends raw requests to the printer.
    /// </summary>
    /// <param name="deviceId">Id of the printer</param>
    /// <param name="request"><see cref="RawRequest"/> to be executed</param>
    /// <returns>Device status with raw response</returns>
    [Post("/printers/{deviceId}/rawrequest")]
    Task<DeviceStatusWithRawResponse> SendRawRequestAsync(string deviceId, RawRequest request);

    /// <summary>
    /// Prints X or Z report.
    /// </summary>
    /// <param name="deviceId">Id of the printer</param>
    /// <param name="action">Action to be performed: <b>xreport</b> or <b>zreport</b>.</param>
    /// <returns>Device status</returns>
    [Post("/printers/{deviceId}/{action}")]
    Task<DeviceStatusWithDateTime> PrintReportAsync(string deviceId, string action);

    /// <summary>
    /// Sends time synchronization request to the printer.
    /// </summary>
    /// <param name="deviceId">Id of the printer</param>
    /// <param name="currentTime">Date and time (<see cref="CurrentDateTime"/>)</param>
    /// <returns></returns>
    [Post("/printers/{deviceId}/datetime")]
    Task<DeviceStatus> SentPrinterTimeAsync(string deviceId, CurrentDateTime currentTime);

}
