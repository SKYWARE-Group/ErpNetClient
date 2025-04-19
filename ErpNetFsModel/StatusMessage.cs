using System.Text.Json.Serialization;

namespace Skyware.ErpNetFS.Model;

/// <summary>
/// Represents a status message with type, code, and text.
/// </summary>
public class StatusMessage
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StatusMessageType Type { get; set; } = StatusMessageType.Unknown;

    // Error and Warning Codes are strings with length of 5 characters.
    // First 3 characters are the type of the error, i.e., ERR, WRN.
    // Next 2 characters are digits, representing the ID of the error or warning. 
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Code { get; set; }

    public string Text { get; set; } = string.Empty;

}
