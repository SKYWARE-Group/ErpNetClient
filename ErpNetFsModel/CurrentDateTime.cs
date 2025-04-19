namespace Skyware.ErpNetFS.Model;

/// <summary>
/// Represents the current date and time of the device.
/// </summary>
public class CurrentDateTime : Credentials
{

    public System.DateTime DeviceDateTime = System.DateTime.Now;

}
