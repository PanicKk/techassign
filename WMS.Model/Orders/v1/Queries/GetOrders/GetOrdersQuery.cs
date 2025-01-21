using MediatR;
using WMS.Models.Common;
using WMS.Models.Common.Pagination;

namespace WMS.Models.Orders.v1.Queries.GetOrders;

public class GetOrdersQuery : BaseFilter, IRequest<IPagedList<OrderListModel>> {}