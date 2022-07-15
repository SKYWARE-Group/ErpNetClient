using Skyware.ErpNetFS.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Skyware.ErpNetFS
{

    public class Client
    {

        private static JsonSerializerOptions serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public string BaseUrl { get; set; } = "http://localhost:8001/";

        public string DeviceId  { get; set; } = string.Empty;

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

        /// <summary>
        /// Retreives list of configured printers
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
        /// Retreives printer status
        /// </summary>
        /// <param name="deviceId">Id of the printer</param>
        /// <returns>The status</returns>
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

    }

}
