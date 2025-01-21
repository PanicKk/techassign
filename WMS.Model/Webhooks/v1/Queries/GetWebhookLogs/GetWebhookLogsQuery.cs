using System.Text.Json.Serialization;
using MediatR;
using WMS.Models.Common;

namespace WMS.Models.Webhooks.v1.Queries.GetWebhookLogs;

public class GetWebhookLogsQuery : BaseFilter, IRequest<WebhookAcitvityLogModel>
{
    [JsonIgnore]
    public string WebhookId { get; set; }
}