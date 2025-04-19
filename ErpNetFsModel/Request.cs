namespace Skyware.ErpNetFS.Model;

/// <summary>
/// Raw Request. 
/// </summary>
public class RequestFrame
{

    /// <summary>
    /// The Raw request, including the command prefix
    /// </summary>
    public string RawRequest { get; set; } = string.Empty;

}