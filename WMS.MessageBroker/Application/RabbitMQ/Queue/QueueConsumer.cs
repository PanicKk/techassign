using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WMS.MessageBroker.Abstraction;
using WMS.MessageBroker.Configuration;

namespace WMS.MessageBroker.Application.RabbitMQ.Queue;

public class QueueConsumer<T> where T : IMessage
{
    private readonly RabbitMqConfiguration _configuration;
    private IConnection _connection;
    private IModel _model;
    private string _queueName;
    private Func<T, Task<bool>> _function;

    public QueueConsumer(string queueName, Func<T, Task<bool>> function, RabbitMqConfiguration configuration)
    {
        _queueName = queueName;
        _configuration = configuration;
        _function = function;
        
        Initialize();
    }

    private void Initialize()
    {
        lock (this)
        {
            if (_model != null)
                _model.Abort();

            if (_connection != null)
                _connection.Abort();

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = _configuration.Username,
                Password = _configuration.Password,
                VirtualHost = _configuration.VirtualHost,
                AutomaticRecoveryEnabled = true,
                RequestedHeartbeat = TimeSpan.FromSeconds(30),
                TopologyRecoveryEnabled = true
            };

            factory.DispatchConsumersAsync = true;

            _connection = factory.CreateConnection();

            _connection.ConnectionBlocked += OnConnectionBlocked;
            _connection.ConnectionShutdown += OnConnectionShutdown;

            _model = _connection.CreateModel();

            _model.BasicQos(0, 10, false);
            _model.QueueDeclare(_queueName, true, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (modelParam, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var obj = JsonConvert.DeserializeObject<T>(message);
                if (await _function(obj))
                    _model.BasicAck(ea.DeliveryTag, false);
            };

            _model.BasicConsume(_queueName, false, string.Empty, false, false, null, consumer);
        }
    }

    private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
    {
        Console.WriteLine("A RabbitMQ queue consumer connection is shutdown. RabbitMq is handling reconnection.");
    }

    private void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
    {
        Console.WriteLine("A RabbitMQ queue consumer connection is shutdown. RabbitMq is handling reconnection.");
    }
}