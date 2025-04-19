﻿namespace Skyware.ErpNetFS.Model;

/// <summary>
/// Represents credentials for the operator.
/// </summary>
public class Credentials
{

    /// <summary>
    /// Operator Name or Operator ID.
    /// </summary>
    public string Operator { get; set; } = string.Empty;

    /// <summary>
    /// Operator Password.
    /// </summary>
    public string OperatorPassword { get; set; } = string.Empty;

}