// Ignore Spelling: taskinfo rawrequest reversalreceipt Erp

using Skyware.ErpNetFS.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Skyware.ErpNetFS
{

    public class Client
    {

        private static readonly JsonSerializerOptions serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public string BaseUrl { get; set; } = "http://localhost:8001/";

        public string DeviceId { get; set; } = string.Empty;



        /// <summary>
        /// Retrieves list of configured printers
        /// </summary>
        /// <returns>List of configured printers</returns>
        public async Task<Dictionary<string, DeviceInfo>> GetPrintersAsync()
        {
            using (var clt = new HttpClient())
            {
                clt.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await clt.GetAsync(new Uri($"{BaseUrl}printers"));
                string resStr = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Dictionary<string, DeviceInfo>>(resStr, serializeOptions);
            }
        }

        /// <summary>
        /// Retrieves printer information - model, uri, etc.
        /// </summary>
        /// <param name="deviceId">Id of the printer</param>
        /// <returns>Information for the device</returns>
        public async Task<DeviceInfo> GetPrinterInfoAsync(string deviceId = null)
        {
            using (var clt = new HttpClient())
            {
                clt.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await clt.GetAsync(new Uri($"{BaseUrl}printers/{(string.IsNullOrWhiteSpace(deviceId) ? this.DeviceId : deviceId)}"));
                string resStr = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<DeviceInfo>(resStr, serializeOptions);
            }
        }

        /// <summary>
        /// Retrieves printer status
        /// </summary>
        /// <param name="deviceId">Id of the printer</param>
        /// <returns>Device status</returns>
        public async Task<DeviceStatusWithDateTime> GetPrinterStatusAsync(string deviceId = null)
        {
            using (var clt = new HttpClient())
            {
                clt.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await clt.GetAsync(new Uri($"{BaseUrl}printers/{(string.IsNullOrWhiteSpace(deviceId) ? this.DeviceId : deviceId)}/status"));
                string resStr = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<DeviceStatusWithDateTime>(resStr, serializeOptions);
            }
        }

        // Get {id}/cash 
        // GET taskinfo : TaskInfoResult

        /// <summary>
        /// Sends raw requests to the printer
        /// </summary>
        /// <param name="request"><see cref="RawRequest"/> to be sent.</param>
        /// <returns>Device status with raw response</returns>
        public async Task<DeviceStatusWithRawResponse> SendRawRequestAsync(RawRequest request)
        {
            using (var clt = new HttpClient())
            {
                StringContent cont = new StringContent(JsonSerializer.Serialize<RawRequest>(request, serializeOptions), Encoding.UTF8, "application/json");
                clt.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await clt.PostAsync(new Uri($"{BaseUrl}printers/{this.DeviceId}/rawrequest"), cont);
                string resStr = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<DeviceStatusWithRawResponse>(resStr, serializeOptions);
            }
        }

        /// <summary>
        /// Register sale
        /// </summary>
        /// <param name="receipt">Receipt</param>
        /// <returns>Receipt status</returns>
        public async Task<DeviceStatusWithReceiptInfo> PrintFiscalReceiptAsync(Receipt receipt)
        {
            using (var clt = new HttpClient())
            {
                StringContent cont = new StringContent(JsonSerializer.Serialize<Receipt>(receipt, serializeOptions), Encoding.UTF8, "application/json");
                clt.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await clt.PostAsync(new Uri($"{BaseUrl}printers/{this.DeviceId}/receipt"), cont);
                string resStr = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<DeviceStatusWithReceiptInfo>(resStr, serializeOptions);
            }
        }

        /// <summary>
        /// Register refund
        /// </summary>
        /// <param name="receipt">Receipt</param>
        /// <returns>Receipt status</returns>
        public async Task<DeviceStatusWithReceiptInfo> PrintRefundReceiptAsync(ReversalReceipt receipt)
        {
            using (var clt = new HttpClient())
            {
                StringContent cont = new StringContent(JsonSerializer.Serialize<ReversalReceipt>(receipt, serializeOptions), Encoding.UTF8, "application/json");
                clt.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await clt.PostAsync(new Uri($"{BaseUrl}printers/{this.DeviceId}/reversalreceipt"), cont);
                string resStr = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<DeviceStatusWithReceiptInfo>(resStr, serializeOptions);
            }
        }

        // POST {id}/withdraw
        // POST {id}/deposit
        // POST {id}/datetime

        /// <summary>
        /// Z Report ends the sales day and can be used for bookkeeping
        /// </summary>
        /// <returns>Device status</returns>
        public async Task<DeviceStatusWithDateTime> PrintZReportAsync()
        {
            return await PrintReportAsync(true);
        }

        /// <summary>
        /// X Report prints sales summary (without closing the fiscal day)
        /// </summary>
        /// <returns>Device status</returns>
        public async Task<DeviceStatusWithDateTime> PrintXReportAsync()
        {
            return await PrintReportAsync(false);
        }

        // POST {id}/duplicate
        // POST {id}/reset

        /// <summary>
        /// Base function (to wrap it)
        /// </summary>
        /// <param name="closeDay"></param>
        /// <returns></returns>
        private async Task<DeviceStatusWithDateTime> PrintReportAsync(bool closeDay)
        {
            using (var clt = new HttpClient())
            {
                StringContent cont = new StringContent(String.Empty, Encoding.UTF8, "application/json");
                clt.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await clt.PostAsync(new Uri($"{BaseUrl}printers/{this.DeviceId}/{(closeDay ? "zreport" : "xreport")}"), cont);
                string resStr = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<DeviceStatusWithDateTime>(resStr, serializeOptions);
            }
        }

    }

}
