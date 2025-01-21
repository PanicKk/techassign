using System.Runtime.Serialization;

namespace WMS.Models.Enums;

public enum AuthenticationType
{
    [EnumMember(Value = "none")]
    None,
    [EnumMember(Value = "basic")]
    Basic,
    [EnumMember(Value = "bearer")]
    Bearer
}