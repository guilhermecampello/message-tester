using System;
using System.Text;
using ABnet.Messages;
using ABnet.Messages.PLC;
using RabbitMQ.Client;

namespace ABnet.MessageTester
{
    public class RabbitMQMessageHandler
    {
        private IConnection connection;

        private IModel channel;

        private string queueName;

        public RabbitMQMessageHandler()
        {
            this.Serializer = new MessageSerializer();
            this.ConnectionFactory = new ConnectionFactory();
            ConnectionFactory.HostName = "localhost";
            ConnectionFactory.Password = "guest";
            ConnectionFactory.UserName = "guest";
            ConnectionFactory.Port = 5672;
            Connect();
        }

        public ConnectionFactory ConnectionFactory { get; set; }

        public MessageSerializer Serializer { get; set; }

        public bool IsConnected
        {
            get
            {
                if(connection != null)
                {
                    return connection.IsOpen;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the connection to the message broker.
        /// </summary>
        public IConnection Connection
        {
            get { return this.connection; }
            set { this.connection = value; }
        }

        public void Connect()
        {
            connection = ConnectionFactory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDelete("tester");
            channel.ExchangeDeclare("tester", "fanout");   
        }

        public void Disconnect()
        {
            connection.Close();
        }

        public void Connect(string hostname, string username, string password, int port, string queueName)
        {
            ConnectionFactory.HostName = hostname;
            ConnectionFactory.UserName = username;
            ConnectionFactory.Password = password;
            ConnectionFactory.Port = port;
            connection = ConnectionFactory.CreateConnection();
            channel = connection.CreateModel();
            this.queueName = queueName;
        }

        public string GetMessage()
        {
            return null;
        }

        public string SendMessage(string message)
        {
            return SendMessage(this.Serializer.Deserialize<SendDestinationMessage>(message));
        }

        public string SendMessage(Message message)
        {
            var exchange = "tester";
            var encriptedMessage = Encoding.UTF8.GetBytes(Serializer.Serialize(message));
            channel.BasicPublish(exchange, "", channel.CreateBasicProperties(), encriptedMessage);

            return string.Empty;
        }
    }
}
