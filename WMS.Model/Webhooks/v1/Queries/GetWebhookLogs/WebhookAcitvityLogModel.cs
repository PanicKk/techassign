using WMS.Models.Enums;

namespace WMS.Models.Webhooks.v1.Queries.GetWebhookLogs;

public class WebhookAcitvityLogModel
{
    public Guid WebhookId { get; set; }
    public EntityType EntityType { get; set; }
    public EventType EventType { get; set; }
    public DateTime RequestDateTime { get; set; }
    public string ResponseStatusCode { get; set; }
    public bool Delivered { get; set; }
}