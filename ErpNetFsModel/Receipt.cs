﻿using System.Collections.Generic;

namespace Skyware.ErpNetFS.Model;


/// <summary>
/// Represents one Receipt, which can be printed on a fiscal printer.
/// </summary>
public class Receipt : Credentials
{

    /// <summary>
    /// The unique sale number is a fiscally controlled number.
    /// </summary>
    public string UniqueSaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// The line items of the receipt.
    /// </summary>
    public IList<Item> Items { get; set; }

    /// <summary>
    /// The payments of the receipt. 
    /// The total amount should match the total amount of the line items.
    /// </summary>
    public IList<Payment> Payments { get; set; }

}