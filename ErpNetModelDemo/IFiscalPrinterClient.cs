using Refit;
using Skyware.ErpNetFS.Model;

namespace ErpNetModelDemo;

// Ignore Spelling: rawrequest reversalreceipt

public interface IFiscalPrinterClient
{

    [Get(Paths.PRINTERS)]
    Task<Dictionary<string, DeviceInfo>> GetPrintersAsync();

    [Get(Paths.PRINTERS + "/{deviceId}")]
    Task<DeviceInfo> GetPrinterAsync(string deviceId);

    [Get(Paths.PRINTERS + "/{deviceId}/status")]
    Task<DeviceStatusWithDateTime> GetPrinterStatusAsync(string deviceId);

    [Post(Paths.PRINTERS + "/{deviceId}/receipt")]
    Task<DeviceStatusWithReceiptInfo> PrintFiscalReceiptAsync(string deviceId, Receipt receipt);

    [Post(Paths.PRINTERS + "/{deviceId}/reversalreceipt")]
    Task<DeviceStatusWithReceiptInfo> PrintRefundReceiptAsync(string deviceId, ReversalReceipt receipt);

    [Post(Paths.PRINTERS + "/{deviceId}/rawrequest")]
    Task<DeviceStatusWithRawResponse> SendRawRequestAsync(string deviceId, RawRequest request);

    [Post(Paths.PRINTERS + "/{deviceId}/{action}")]
    Task<DeviceStatusWithDateTime> PrintReportAsync(string deviceId, string action);

}
