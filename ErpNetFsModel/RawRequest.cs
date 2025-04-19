using System.Text.Json.Serialization;

namespace Skyware.ErpNetFS.Model;


/// <summary>
/// Represent raw request (command+data) to be sent to the fiscal printer
/// </summary>
public class RawRequest
{

    /// <summary>
    /// The raw command to be sent to the fiscal printer.
    /// </summary>
    [JsonPropertyName("rawRequest")]
    public string Request { get; set; }

}
