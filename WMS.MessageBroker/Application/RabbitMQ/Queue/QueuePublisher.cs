using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using WMS.MessageBroker.Configuration;

namespace WMS.MessageBroker.Application.RabbitMQ.Queue;

public class QueuePublisher
{
    private readonly RabbitMqConfiguration _configuration;

    private static readonly ManualResetEvent _doorman = new ManualResetEvent(false);
    private static readonly object _locker = new object();
    private IConnection _connection;
    private IModel _model;
    private Dictionary<string, DateTime> _queueDeclares = new Dictionary<string, DateTime>();

    public QueuePublisher(RabbitMqConfiguration configuration)
    {
        _configuration = configuration;
        
        Initialize();
    }

    private void Initialize()
    {
        lock (_locker)
        {
            _doorman.Reset();
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

            if (_model != null)
                _model.Abort();

            if (_connection != null)
                _connection.Abort();

            _connection = factory.CreateConnection();

            _connection.ConnectionBlocked += OnConnectionBlocked;
            _connection.ConnectionShutdown += OnConnectionShutdown;

            _model = _connection.CreateModel();

            _queueDeclares.Clear();
            _doorman.Set();
        }
    }

    public void Publish(string queueName, object obj, int tryCount = 0)
    {
        try
        {
            _doorman.WaitOne(TimeSpan.FromSeconds(5));
            if (!_queueDeclares.ContainsKey(queueName)
                || _queueDeclares[queueName] < DateTime.Now.AddMinutes(-5))
            {
                _model.QueueDeclare(queueName, true, false, false, null);
                _queueDeclares[queueName] = DateTime.Now;
            }

            var properties = _model.CreateBasicProperties();
            properties.DeliveryMode = 2; // persistent messages

            var json = JsonConvert.SerializeObject(obj);
            _model.BasicPublish(string.Empty, queueName, true, properties, Encoding.UTF8.GetBytes(json));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            if (tryCount < 5)
            {
                Thread.Sleep(200);
                Publish(queueName, obj, tryCount + 1);
            }
        }
    }

    private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
    {
        Console.WriteLine("A RabbitMQ queue publisher connection is shutdown. RabbitMq is handling reconnection.");
    }

    private void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
    {
        Console.WriteLine("A RabbitMQ queue publisher connection is shutdown. RabbitMq is handling reconnection.");
    }
}