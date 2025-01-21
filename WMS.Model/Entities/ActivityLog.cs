using WMS.Models.Entities.Shared;
using WMS.Models.Enums;

namespace WMS.Models.Entities;

public class ActivityLog : BaseEntity
{
    public Guid WebhookId { get; set; }
    public EntityType EntityType { get; set; }
    public EventType EventType { get; set; }
    public DateTime RequestDateTime { get; set; }
    public string ResponseStatusCode { get; set; }
    public bool Delivered { get; set; }
}