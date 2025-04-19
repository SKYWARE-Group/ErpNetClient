using System.Text.Json.Serialization;

namespace Skyware.ErpNetFS.Model;

/// <summary>
/// Represents one line in a receipt. 
/// Can be either a comment or a fiscal line.
/// </summary>
public class Item
{
    public int ItemCode { get; set; } = 999;
    /// <summary>
    /// ItemType is the type of the item row
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ItemType Type { get; set; } = ItemType.Sale;

    /// <summary>
    /// Gets or sets the text of the line.
    /// </summary>
    /// <value>
    /// The text.
    /// </value>
    public string Text { get; set; } = "";

    /// <summary>
    /// Gets or sets the tax group. 
    /// </summary>
    /// <value>
    /// The tax group.
    /// </value>        
    public TaxGroup TaxGroup { get; set; } = TaxGroup.Unspecified;

    /// <summary>
    /// Gets or sets the department. Department 0 means no department.
    /// </summary>
    /// <value>
    /// The department.
    /// </value>
    public int Department { get; set; } = 0;

    /// <summary>
    /// Gets or sets the quantity.
    /// </summary>
    /// <value>
    /// The quantity.
    /// </value>
    public decimal Quantity { get; set; } = 0m;

    /// <summary>
    /// Gets or sets the unit price.
    /// </summary>
    /// <value>
    /// The unit price.
    /// </value>
    public decimal UnitPrice { get; set; } = 0m;

    /// <summary>
    /// Gets or sets the amount. Used in discount and surcharge amount types.
    /// </summary>
    /// <value>
    /// The amount of discount or the amount of surcharge
    /// </value>
    public decimal Amount { get; set; } = 0m;

    /// <summary>
    /// Gets or sets the discounts, surcharges.
    /// </summary>
    /// <value>
    /// The Price Modifier Value.
    /// </value>
    public decimal PriceModifierValue { get; set; } = 0m;

    /// <summary>
    /// Get or sets the PriceModifierType, None is default
    /// </summary>
    /// <value>
    /// The Price Modifier Type
    /// </value>
    /// <seealso cref="Model.PriceModifierType"/>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PriceModifierType PriceModifierType { get; set; } = PriceModifierType.None;

}