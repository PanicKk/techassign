using WMS.Models.Enums;

namespace WMS.Models.Webhooks.v1.Queries.GetWebhooks;

public class WebhookListModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public EntityType EntityType { get; set; }
    public List<EventType> EventTypes { get; set; }
    public bool IsActive { get; set; }
}