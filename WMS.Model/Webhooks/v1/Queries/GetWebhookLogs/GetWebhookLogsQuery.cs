using System.Text.Json.Serialization;
using MediatR;
using WMS.Models.Common;
using WMS.Models.Common.Pagination;

namespace WMS.Models.Webhooks.v1.Queries.GetWebhookLogs;

public class GetWebhookLogsQuery : BaseFilter, IRequest<IPagedList<WebhookAcitvityLogModel>>
{
    [JsonIgnore]
    public Guid WebhookId { get; set; }
}