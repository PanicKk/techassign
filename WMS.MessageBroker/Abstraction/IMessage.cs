using System.Text.Json.Serialization;

namespace WMS.MessageBroker.Abstraction;

public interface IMessage
{
    [JsonIgnore]
    public string MessageName { get; }
}