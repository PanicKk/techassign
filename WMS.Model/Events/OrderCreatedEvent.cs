using Newtonsoft.Json;
using WMS.MessageBroker.Abstraction;
using WMS.Models.Enums;

namespace WMS.Models.Events;

public class OrderCreatedEvent<T> : IMessage where T : class
{
    [JsonIgnore]
    public string MessageName => "order.created.v1";
    public T Data { get; set; }
}