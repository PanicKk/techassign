using WMS.MessageBroker.Abstraction;

namespace WMS.Core.Services.IServices;

public interface IPublisherService
{
    void PublishToQueue(IMessage message);
}