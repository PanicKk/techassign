using System.Net;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using WMS.Core.Exceptions;
using WMS.Core.Repositories;
using WMS.Core.Services.IServices;
using WMS.Models.Entities;
using WMS.Models.Enums;
using WMS.Models.Events;
using WMS.Models.Orders.v1.Commands.UpdateOrder;
using WMS.Models.Orders.v1.Shared;

namespace WMS.Core.Handlers.Orders.v1.Commands;

public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, UpdateOrderResponse>
{
    private readonly ILogger<UpdateOrderHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IRepository<Order> _orderRepository;
    private readonly IPublisherService _publisherService;

    public UpdateOrderHandler(ILogger<UpdateOrderHandler> logger, IMapper mapper, IRepository<Order> orderRepository,
                              IPublisherService publisherService)
    {
        _logger = logger;
        _mapper = mapper;
        _orderRepository = orderRepository;
        _publisherService = publisherService;
    }

    public async Task<UpdateOrderResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetAsync(query => query.Where(order => order.Id == request.Id));

            if (order == null)
                throw new WMSException("Order not found! OrderId: {request.Id}", ExceptionType.OrderNotFound,
                                       HttpStatusCode.NotFound);

            order.Items = _mapper.Map<List<OrderItem>>(request.Items);

            await _orderRepository.UpdateAsync(order);

            var updateOrderResponse = new UpdateOrderResponse
            {
                Items = _mapper.Map<List<OrderItemModel>>(order.Items),
                OrderUpdated = true,
                TotalAmount = order.Items.Sum(orderItem => orderItem.Price * orderItem.Quantity),
            };

            PublishOrderUpdated(updateOrderResponse);

            return updateOrderResponse;
        }
        catch (Exception ex)
        {
            if (ex is WMSException)
                throw;
            
            _logger.LogError(ex, "Failed to Update Order! OrderId: {OrderId}, Message: {Message}", request.Id, ex.Message);

            throw new WMSException("Failed to Update Order!", ExceptionType.NotUpdated, HttpStatusCode.InternalServerError);
        }
    }

    private void PublishOrderUpdated(UpdateOrderResponse updateOrderResponse)
    {
        var orderUpdatedEvent = new OrderUpdatedEvent<UpdateOrderResponse>()
        {
            Data = updateOrderResponse
        };

        _publisherService.PublishToQueue(orderUpdatedEvent);
    }
}