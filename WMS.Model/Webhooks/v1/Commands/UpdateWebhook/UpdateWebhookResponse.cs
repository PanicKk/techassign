using WMS.Models.Webhooks.v1.Shared;

namespace WMS.Models.Webhooks.v1.Commands.UpdateWebhook;

public class UpdateWebhookResponse
{
    public bool Updated { get; set; }
    public WebhookModel Webhook { get; set; }
}