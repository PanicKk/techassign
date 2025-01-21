using WMS.Models.Webhooks.v1.Shared;

namespace WMS.Models.Webhooks.v1.Commands.DeleteWebhook;

public class DeleteWebhookResponse
{
    public bool Deleted { get; set; }
    public WebhookModel Webhook { get; set; }
}