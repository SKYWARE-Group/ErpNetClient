using System;

namespace Skyware.ErpNetFS.Model;

/// <summary>
/// Device status that includes receipt information.
/// </summary>
public class DeviceStatusWithReceiptInfo : DeviceStatus
{

    /// <summary>
    /// The receipt number.
    /// </summary>
    public string ReceiptNumber { get; set; } = string.Empty;

    /// <summary>
    /// The receipt date and time.
    /// </summary>
    public DateTime ReceiptDateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// The receipt amount.
    /// </summary>
    public decimal ReceiptAmount { get; set; } = 0m;

    /// <summary>
    /// The fiscal memory number.
    /// </summary>
    public string FiscalMemorySerialNumber { get; set; } = string.Empty;

}
