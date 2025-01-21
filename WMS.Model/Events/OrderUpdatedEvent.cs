using Newtonsoft.Json;
using WMS.MessageBroker.Abstraction;

namespace WMS.Models.Events;

public class OrderUpdatedEvent<T> : IMessage where T : class
{
    [JsonIgnore] public string MessageName => "order.updated.v1";
    public T Data { get; set; }
}