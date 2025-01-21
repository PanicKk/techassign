using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WMS.Core.Services.IServices;
using WMS.MessageBroker.Abstraction;
using WMS.MessageBroker.Application.RabbitMQ.Queue;
using WMS.MessageBroker.Configuration;
using WMS.Models.Enums;
using WMS.Models.Events;
using WMS.Models.Orders.v1.Commands.UpdateOrder;

namespace WMS.Core.Consumers;

public class OrderUpdatedEventConsumer : IConsumer
{
    private readonly ILogger<OrderUpdatedEventConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly QueueConsumer<OrderUpdatedEvent<UpdateOrderResponse>> _queueConsumer;


    public OrderUpdatedEventConsumer(
        ILogger<OrderUpdatedEventConsumer> logger,
        IServiceProvider serviceProvider,
        RabbitMqConfiguration rabbitMqConfiguration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;

        _queueConsumer = new QueueConsumer<OrderUpdatedEvent<UpdateOrderResponse>>(
            new OrderUpdatedEvent<UpdateOrderResponse>().MessageName, MessageReceived,
            rabbitMqConfiguration);
    }

    private async Task<bool> MessageReceived(OrderUpdatedEvent<UpdateOrderResponse> orderUpdatedEvent)
    {
        using var scope = _serviceProvider.CreateScope();
        var webhookDeliveryService = scope.ServiceProvider.GetService<IWebhookDeliveryService>();

        await webhookDeliveryService.HandleEvent(EntityType.Order, EventType.Updated, orderUpdatedEvent.Data);

        return true;
    }
}