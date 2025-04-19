namespace Skyware.ErpNetFS.Model;

/// <summary>
/// Device status that includes raw response data.
/// </summary>
public class DeviceStatusWithRawResponse : DeviceStatus
{

    /// <summary>
    /// The raw response data from the device.
    /// </summary>
    public string RawResponse { get; set; } = string.Empty;

}
