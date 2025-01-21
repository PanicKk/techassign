using RabbitMQ.Client;

namespace WMS.MessageBroker.Configuration;

public class RabbitMqConfiguration
{
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string VirtualHost { get; set; }
}