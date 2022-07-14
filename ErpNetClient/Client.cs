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
        public async Task<DeviceStatusWithReceiptInfo> PrintFiscalReceipt(Receipt receipt)
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

        private async Task<DeviceStatusWithDateTime> PrintReport(bool closeDay)
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
        public async Task<DeviceStatusWithDateTime> PrintZReport()
        {
            return await PrintReport(true);
        }

        /// <summary>
        /// X Report shows sales summary
        /// </summary>
        /// <returns>Device status</returns>
        public async Task<DeviceStatusWithDateTime> PrintXReport()
        {
            return await PrintReport(false);
        }


        public async Task<Dictionary<string, DeviceInfo>> GetPrinters()
        {
            using (var clt = new HttpClient())
            {
                clt.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage res = await clt.GetAsync(new Uri($"{BaseUrl}printers"));
                string resStr = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Dictionary<string, DeviceInfo>>(resStr, serializeOptions);
            }
        }

        public async Task<DeviceStatusWithDateTime> GetPrinterStatus(string deviceId = null)
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
