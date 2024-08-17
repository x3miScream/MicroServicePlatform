using System.Text;
using System.Text.Json;
using PlatformService.DTOs;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;

            var factory = new ConnectionFactory(){
                HostName = _configuration["RabbitMQHost"],
                Port = Convert.ToInt32(_configuration["RabbitMQPort"]),

                UserName = "iScream",
                Password = "iScream",

                ContinuationTimeout = new TimeSpan(10,0,0,0)
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown;

                Console.WriteLine($"Connected to Message Bus");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Failed to connect to the Message Bus {ex.Message}");

                if(ex.InnerException != null)
                {
                    Console.WriteLine($"--> Inner Exception {ex.InnerException.Message}");
                }
            }
        }

        public void PublishNewPlatform(PlatformPublishedDTO platformPublishedDTO)
        {
            var message = JsonSerializer.Serialize(platformPublishedDTO);

            if(_connection.IsOpen)
            {
                Console.WriteLine("--> Rabbit MQ Connection is Open, Sending message...");

                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> Rabbit MQ Connection is Closed, not sending...");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger", 
                        routingKey: "",
                        basicProperties: null,
                        body: body);

            Console.WriteLine($"--> Send message: {message} to Rabbit MQ");
        }

        public void Dispose()
        {
            Console.WriteLine("--> Message Bud Disposed");

            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void RabbitMQ_ConnectionShutDown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"--> Rabbit MQ Connection Shutdown");
        }
    }
}