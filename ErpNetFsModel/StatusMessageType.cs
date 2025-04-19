using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Skyware.ErpNetFS.Model;

/// <summary>
/// Defines the type of status message.
/// </summary>
public enum StatusMessageType
{

    [EnumMember(Value = "")]
    Unknown,

    [EnumMember(Value = "reserved")]
    Reserved,

    [EnumMember(Value = "info")]
    Info,

    [EnumMember(Value = "warning")]
    Warning,

    [EnumMember(Value = "error")]
    Error

}
