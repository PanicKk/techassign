using System.Text.Json.Serialization;
using MediatR;
using WMS.Models.Webhooks.v1.Shared;

namespace WMS.Models.Webhooks.v1.Queries.GetWebhook;

public class GetWebhookQuery : IRequest<WebhookModel>
{
    [JsonIgnore]
    public Guid Id { get; set; }
}