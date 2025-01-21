using System.Runtime.Serialization;

namespace WMS.Models.Enums;

public enum OrderStatus
{
    [EnumMember(Value = "pending")]
    Pending = 1,
    [EnumMember(Value = "confirmed")]
    Confirmed = 2,
    [EnumMember(Value = "shipped")]
    Shipped = 3,
    [EnumMember(Value = "delivered")]
    Delivered = 4
}