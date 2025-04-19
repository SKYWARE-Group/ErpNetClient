using System.Runtime.Serialization;

namespace Skyware.ErpNetFS.Model;

public enum ItemType
{

    [EnumMember(Value = "sale")]
    Sale,

    [EnumMember(Value = "comment")]
    Comment,

    [EnumMember(Value = "footer-comment")]
    FooterComment,

    [EnumMember(Value = "surcharge-amount")]
    SurchargeAmount,

    [EnumMember(Value = "discount-amount")]
    DiscountAmount

}
