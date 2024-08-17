using System.Text;
using CommandsService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandsService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;
        private IEventProcessor _eventProcessor;
        private string _queueName;

        public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
        {
            _configuration = configuration;
            _eventProcessor = eventProcessor;

            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
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
                _queueName = _channel.QueueDeclare().QueueName;
                _channel.QueueBind(queue: _queueName, 
                    exchange: "trigger",
                    routingKey: "");

                Console.WriteLine($"--> Listening on the Message Buss... Queue: {_queueName}");

                _connection.ConnectionShutdown += RabbotMQ_ConnectionShutdown;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Failed to initialize Message Bus connection at Commands Service: {ex.Message}");
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) => {

                Console.WriteLine($"--> Event Received");

                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                _eventProcessor.ProcessEvent(notificationMessage);            
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private void RabbotMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("==> Connection Shutdown");
        }

        public override void Dispose()
        {
            if(_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

            base.Dispose();
        }
    }
}