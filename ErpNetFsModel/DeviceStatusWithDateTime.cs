using System;

namespace Skyware.ErpNetFS.Model;

/// <summary>
/// Device status that includes date and time information.
/// </summary>
public class DeviceStatusWithDateTime : DeviceStatus
{

    /// <summary>
    /// The date and time of the device.
    /// </summary>
    public DateTime DeviceDateTime { get; set; }

}
