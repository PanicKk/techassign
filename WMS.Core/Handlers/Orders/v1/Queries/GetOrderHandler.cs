using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WMS.Core.Exceptions;
using WMS.Core.Repositories;
using WMS.Models.Entities;
using WMS.Models.Enums;
using WMS.Models.Orders.v1.Queries.GetOrder;
using WMS.Models.Orders.v1.Shared;

namespace WMS.Core.Handlers.Orders.v1.Queries;

public class GetOrderHandler : IRequestHandler<GetOrderQuery, OrderModel>
{
    private readonly ILogger<GetOrderHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IRepository<Order> _orderRepository;

    public GetOrderHandler(ILogger<GetOrderHandler> logger, IMapper mapper, IRepository<Order> orderRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _orderRepository = orderRepository;
    }

    public async Task<OrderModel> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetAsync(query => query.Where(order => order.Id == request.Id)
                                                                      .Include(order => order.Items));

            if (order == null)
                throw new WMSException("Order not found! OrderId: {request.Id}", ExceptionType.OrderNotFound,
                                       HttpStatusCode.NotFound);

            var orderModel = _mapper.Map<OrderModel>(order);

            return orderModel;
        }
        catch (Exception ex)
        {
            if (ex is WMSException)
                throw;
            
            _logger.LogError(ex, "Failed to Get Order! OrderId: {OrderId}, Message: {Message}", request.Id, ex.Message);

            throw new WMSException("Failed to Get Order!", ExceptionType.ServerError, HttpStatusCode.InternalServerError);
        }
    }
}