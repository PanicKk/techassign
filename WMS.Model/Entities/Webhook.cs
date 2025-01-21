using WMS.Models.Entities.Shared;
using WMS.Models.Enums;

namespace WMS.Models.Entities;

public class Webhook : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public EntityType EntityType { get; set; }
    public List<EventType> EventTypes { get; set; }
    public string Url { get; set; }
    public Dictionary<string, string> CustomPayload { get; set; }
    public AuthenticationType AuthType { get; set; }
    public string Token { get; set; }
    public bool IsActive { get; set; }
}