﻿using System.Collections.Generic;

namespace Skyware.ErpNetFS.Model;

/// <summary>
/// Represents information about a fiscal device.
/// </summary>
public class DeviceInfo
{

    /// <summary>
    /// Fiscal printer URI.
    /// </summary>
    public string Uri { get; set; } = string.Empty;

    /// <summary>
    /// Fiscal printer serial number.
    /// </summary>
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// Fiscal printer memory serial number.
    /// </summary>
    public string FiscalMemorySerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// Manufacturer - Company or Trademark of Company that produces the fiscal device.
    /// </summary>
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// Device model.
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Optional. Firmware version.
    /// </summary>
    public string FirmwareVersion { get; set; } = string.Empty;

    /// <summary>
    /// Maximum symbols for operator names, item names, department names allowed.
    /// </summary>
    public int ItemTextMaxLength { get; set; }

    /// <summary>
    /// Maximum symbols for payment names allowed.
    /// </summary>
    public int CommentTextMaxLength { get; set; }

    /// <summary>
    /// Maximal operator password length allowed.
    /// </summary>
    public int OperatorPasswordMaxLength { get; set; }

    /// <summary>
    /// Tax Number is Fiscal Subject Identification Number.
    /// </summary>
    public string TaxIdentificationNumber { get; set; } = string.Empty;

    /// <summary>
    /// List of supported payment types by the device.
    /// </summary>  
    public IList<string> SupportedPaymentTypes { get; set; }

    /// <summary>
    /// Expresses support of item types discount-amount and surcharge-amount by the device.
    /// </summary>   
    public bool SupportsSubTotalAmountModifiers { get; set; } = false;

    /// <summary>
    /// Expresses support of payment terminal for current device model.
    /// </summary>
    public bool SupportPaymentTerminal { get; set; } = false;

    /// <summary>
    /// Expresses using of payment terminal for current device.
    /// </summary>
    public bool UsePaymentTerminal { get; set; } = false;

}