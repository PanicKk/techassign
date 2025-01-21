using System.Runtime.Serialization;

namespace WMS.Models.Enums;

public enum EventType
{
    [EnumMember(Value = "created")]
    Created = 1,
    [EnumMember(Value = "updated")]
    Updated = 2,
    [EnumMember(Value = "deleted")]
    Deleted = 3
}