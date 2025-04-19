using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Skyware.ErpNetFS.Model;


/// <summary>
/// Reversal Reason
/// </summary>
public enum ReversalReason
{
    [EnumMember(Value = "operator-error")]
    OperatorError = 1,

    [EnumMember(Value = "refund")]
    Refund = 2,

    [EnumMember(Value = "tax-base-reduction")]
    TaxBaseReduction = 3,

    [EnumMember(Value = "taxbase-reduction")]
    OldCompatibleTaxBaseReduction = 3   // must be same as TaxBaseReduction

}

/// <summary>
/// Represents refund receipt, which can be printed on a fiscal printer.
/// </summary>
public class ReversalReceipt : Receipt
{
    public string ReceiptNumber { get; set; } = string.Empty;

    public System.DateTime ReceiptDateTime { get; set; }

    public string FiscalMemorySerialNumber { get; set; } = string.Empty;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ReversalReason Reason { get; set; } = ReversalReason.OperatorError;

    public ReversalReceipt CloneReceipt(Receipt receipt)
    {
        if (receipt.Items != null)
        {
            Items = receipt.Items.ToList();
        }
        if (receipt.Payments != null)
        {
            Payments = receipt.Payments.ToList();
        }
        UniqueSaleNumber = receipt.UniqueSaleNumber;
        return this;
    }

}