using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WMS.Core.Services.IServices;
using WMS.MessageBroker.Abstraction;
using WMS.MessageBroker.Application.RabbitMQ.Queue;
using WMS.MessageBroker.Configuration;
using WMS.Models.Enums;
using WMS.Models.Events;
using WMS.Models.Orders.v1.Shared;

namespace WMS.Core.Consumers;

public class OrderCreatedEventConsumer : IConsumer
{
    private readonly ILogger<OrderCreatedEventConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly QueueConsumer<OrderCreatedEvent<OrderModel>> _queueConsumer;

    public OrderCreatedEventConsumer(
        ILogger<OrderCreatedEventConsumer> logger,
        IServiceProvider serviceProvider,
        RabbitMqConfiguration rabbitMqConfiguration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;

        _queueConsumer = new QueueConsumer<OrderCreatedEvent<OrderModel>>(
            new OrderCreatedEvent<OrderModel>().MessageName, MessageReceived,
            rabbitMqConfiguration);
    }

    private async Task<bool> MessageReceived(OrderCreatedEvent<OrderModel> orderCreatedEvent)
    {
        using var scope = _serviceProvider.CreateScope();
        var webhookDeliveryService = scope.ServiceProvider.GetService<IWebhookDeliveryService>();

        await webhookDeliveryService.HandleEvent(EntityType.Order, EventType.Created, orderCreatedEvent.Data);

        return true;
    }
}