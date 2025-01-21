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
using WMS.Models.Orders.v1.Commands.CreateOrder;
using WMS.Models.Orders.v1.Shared;

namespace WMS.Core.Handlers.Orders.v1.Commands;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderModel>
{
    private readonly ILogger<CreateOrderHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IRepository<Order> _orderRepository;
    private readonly IPublisherService _publisherService;

    public CreateOrderHandler(ILogger<CreateOrderHandler> logger, IMapper mapper, IRepository<Order> orderRepository, IPublisherService publisherService)
    {
        _logger = logger;
        _mapper = mapper;
        _orderRepository = orderRepository;
        _publisherService = publisherService;
    }

    public async Task<OrderModel> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = _mapper.Map<Order>(request);

            order.TotalAmount = request.Items.Sum(orderItem => orderItem.Price * orderItem.Quantity);
            order.Status = OrderStatus.Pending;

            await _orderRepository.InsertAsync(order);

            var createdOrder = _mapper.Map<OrderModel>(order);

            PublishOrderCreatedEvent(createdOrder);

            return createdOrder;
        }
        catch (Exception ex)
        {
            if (ex is WMSException)
                throw;
            
            _logger.LogError(ex, "Failed to Create Order! Message: {Message}", ex.Message);
            
            throw new WMSException("Failed to Create Order!", ExceptionType.NotCreated, HttpStatusCode.InternalServerError);
        }
    }

    private void PublishOrderCreatedEvent(OrderModel createdOrder)
    {
        var orderCreatedEvent = new OrderCreatedEvent<OrderModel>
        {
            Data = createdOrder
        };
        
        _publisherService.PublishToQueue(orderCreatedEvent);
    }
}