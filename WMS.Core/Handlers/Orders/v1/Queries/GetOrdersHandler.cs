using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WMS.Core.Exceptions;
using WMS.Core.Extensions;
using WMS.Core.Repositories;
using WMS.Models.Common;
using WMS.Models.Common.Pagination;
using WMS.Models.Entities;
using WMS.Models.Enums;
using WMS.Models.Orders.v1.Queries.GetOrders;

namespace WMS.Core.Handlers.Orders.v1.Queries;

public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, IPagedList<OrderListModel>>
{
    private readonly ILogger<GetOrdersHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IRepository<Order> _orderRepository;

    public GetOrdersHandler(ILogger<GetOrdersHandler> logger, IMapper mapper, IRepository<Order> orderRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _orderRepository = orderRepository;
    }

    public async Task<IPagedList<OrderListModel>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var orders = await GetOrders(request);

            var ordersList = orders.Select(order =>
                                   {
                                       var result = _mapper.Map<OrderListModel>(order);
                                       return result;
                                   })
                                   .ToList();

            return new PagedList<OrderListModel>(ordersList, orders.PageIndex, orders.PageSize, orders.TotalCount);
        }
        catch (Exception ex)
        {
            if (ex is WMSException)
                throw;
            
            _logger.LogError(ex, "Failed to Get Orders! Message: {Message}", ex.Message);

            throw new WMSException("Failed to Get Orders!", ExceptionType.ServerError, HttpStatusCode.InternalServerError);
        }
    }

    private async Task<IPagedList<Order>> GetOrders(GetOrdersQuery request)
    {
        var query = _orderRepository.Table.Where(order => !order.IsDeleted);


        query = query.GetOrderedEntities("Id", request.OrderBy, request.SortingOrder ?? SortingOrder.DESC);

        query = query.Include(order => order.Items);

        return await query.ToPagedListAsync(request.PageIndex, request.PageSize);
    }
}