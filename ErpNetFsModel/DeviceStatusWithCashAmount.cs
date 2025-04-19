namespace Skyware.ErpNetFS.Model;

/// <summary>
/// Device status that includes cash amount information.
/// </summary>
public class DeviceStatusWithCashAmount : DeviceStatus
{

    /// <summary>
    /// The cash amount, meaning depends on context.
    /// </summary>
    public decimal Amount { get; set; }

}
