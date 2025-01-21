using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WMS.Core.Services.IServices;
using WMS.MessageBroker.Abstraction;
using WMS.MessageBroker.Application.RabbitMQ.Queue;
using WMS.MessageBroker.Configuration;
using WMS.Models.Enums;
using WMS.Models.Events;
using WMS.Models.Orders.v1.Commands.DeleteOrder;

namespace WMS.Core.Consumers;

public class OrderDeletedEventConsumer : IConsumer
{
    private readonly ILogger<OrderDeletedEventConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly QueueConsumer<OrderDeletedEvent<DeleteOrderResponse>> _queueConsumer;

    public OrderDeletedEventConsumer(ILogger<OrderDeletedEventConsumer> logger, IServiceProvider serviceProvider,
        RabbitMqConfiguration rabbitMqConfiguration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _queueConsumer = new QueueConsumer<OrderDeletedEvent<DeleteOrderResponse>>(
            new OrderDeletedEvent<DeleteOrderResponse>().MessageName, MessageReceived, rabbitMqConfiguration);
    }

    private async Task<bool> MessageReceived(OrderDeletedEvent<DeleteOrderResponse> orderDeletedEvent)
    {
        using var scope = _serviceProvider.CreateScope();
        var webhookDeliveryService = scope.ServiceProvider.GetService<IWebhookDeliveryService>();

        await webhookDeliveryService.HandleEvent(EntityType.Order, EventType.Deleted, orderDeletedEvent.Data);

        return true;
    }
}