using Newtonsoft.Json;
using WMS.MessageBroker.Abstraction;

namespace WMS.Models.Events;

public class OrderDeletedEvent<T> : IMessage where T : class
{
    [JsonIgnore]
    public string MessageName => "order.deleted.v1";
    public T Data { get; set; }
}