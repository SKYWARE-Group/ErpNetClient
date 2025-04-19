using System.Collections.Generic;

namespace Skyware.ErpNetFS.Model;

/// <summary>
/// Represents the base device status.
/// </summary>
public class DeviceStatus
{

    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool Ok { get; protected set; } = true;

    /// <summary>
    /// A list of status messages associated with the device status.
    /// </summary>
    public IList<StatusMessage> Messages { get; protected set; } = [];

}
