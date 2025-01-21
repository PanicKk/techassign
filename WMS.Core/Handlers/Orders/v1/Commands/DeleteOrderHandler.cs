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
using WMS.Models.Orders.v1.Commands.DeleteOrder;
using WMS.Models.Orders.v1.Shared;

namespace WMS.Core.Handlers.Orders.v1.Commands;

public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, DeleteOrderResponse>
{
    private readonly ILogger<DeleteOrderHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IRepository<Order> _orderRepository;
    private readonly IPublisherService _publisherService;

    public DeleteOrderHandler(IMapper mapper, IRepository<Order> orderRepository, IPublisherService publisherService)
    {
        _mapper = mapper;
        _orderRepository = orderRepository;
        _publisherService = publisherService;
    }

    public async Task<DeleteOrderResponse> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetAsync(query => query.Where(order => order.Id == request.Id));

            if (order == null)
                throw new WMSException($"Order not found! OrderId: {request.Id}", ExceptionType.OrderNotFound,
                                       HttpStatusCode.NotFound);

            await _orderRepository.DeleteAsync(order);

            var deleteOrderResponse = new DeleteOrderResponse
            {
                Deleted = true,
                Order = _mapper.Map<OrderModel>(order)
            };
            
            PublishOrderDeletedEvent(deleteOrderResponse);

            return deleteOrderResponse;
        }
        catch (Exception ex)
        {
            if (ex is WMSException)
                throw;
            
            _logger.LogError(ex, "Failed to Delete Order! OrderId: {OrderId}, Message: {Message}", request.Id, ex.Message);

            throw new WMSException("Failed to Delete Order!", ExceptionType.NotDeleted, HttpStatusCode.InternalServerError);
        }
    }
    private void PublishOrderDeletedEvent(DeleteOrderResponse deleteOrderResponse)
    {
        var orderCreatedEvent = new OrderDeletedEvent<DeleteOrderResponse>
        {
            Data = deleteOrderResponse
        };
        
        _publisherService.PublishToQueue(orderCreatedEvent);
    }
    
}