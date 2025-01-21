using MediatR;
using WMS.Models.Common;
using WMS.Models.Common.Pagination;
using WMS.Models.Orders.v1.Queries.GetOrders;

namespace WMS.Models.Webhooks.v1.Queries.GetWebhooks;

public class GetWebhooksQuery : BaseFilter, IRequest<IPagedList<WebhookListModel>> {}