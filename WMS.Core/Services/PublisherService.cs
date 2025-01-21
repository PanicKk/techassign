using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WMS.Core.Services.IServices;
using WMS.MessageBroker.Abstraction;
using WMS.MessageBroker.Application.RabbitMQ.Queue;
using WMS.MessageBroker.Configuration;

namespace WMS.Core.Services;

public class PublisherService : IPublisherService
{
    private readonly ILogger<PublisherService> _logger;
    private readonly QueuePublisher _queuePublisher;

    public PublisherService(ILogger<PublisherService> logger, RabbitMqConfiguration rabbitMqConfiguration)
    {
        _logger = logger;
        _queuePublisher = new QueuePublisher(rabbitMqConfiguration);
    }

    public void PublishToQueue(IMessage message)
    {
        _queuePublisher.Publish(message.MessageName, message);
    }
}