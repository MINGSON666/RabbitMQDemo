using System;
using System.Text;
using RabbitMQ.Client;

namespace Producer
{
    class Producer
    {
        static void Main(string[] args)
        {
            // 所有发送到Direct Exchange的消息被转发到具有指定RouteKey的Queue
            // 所有发送到Fanout Exchange的消息都会被转发到与该Exchange 绑定(Binding)的所有Queue上
            // 所有发送到Topic Exchange的消息被转发到能和Topic匹配的Queue上

            //var directExchange = "directExchange";
            //var directQueue = "directQueue";
            //var directKey = "directKey";

            //var fanoutExchange = "fanoutExchange";
            //var fanoutQueue1 = "fanoutQueue1";
            //var fanoutQueue2 = "fanoutQueue2";
            //var fanoutKey = "";// Fanout Exchange 不需要处理RouteKey

            var topicExchange = "topicExchange";
            var topicQueue = "topicQueue";
            var topicKey = "topicKey.*";// 符号“#”匹配一个或多个词，符号“*”匹配不多不少一个词

            //创建连接工厂
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "guest",//用户名
                Password = "guest",//密码
                HostName = "127.0.0.1"//rabbitmq ip
            };

            //创建连接
            var connection = factory.CreateConnection();

            //创建通道
            var channel = connection.CreateModel();

            //定义交换机
            //channel.ExchangeDeclare(directExchange, ExchangeType.Direct, false, false, null);// Direct模式
            //channel.ExchangeDeclare(fanoutExchange, ExchangeType.Fanout, false, false, null);// Fanout模式
            channel.ExchangeDeclare(topicExchange, ExchangeType.Topic, false, false, null);// Topic模式

            //声明队列
            //channel.QueueDeclare("hello", false, false, false, null);// hello
            //channel.QueueDeclare(directQueue, false, false, false, null);// Direct模式
            //channel.QueueDeclare(fanoutQueue1, false, false, false, null);// Fanout模式
            //channel.QueueDeclare(fanoutQueue2, false, false, false, null);// Fanout模式
            channel.QueueDeclare(topicQueue, false, false, false, null);// Topic模式

            //将队列绑定到交换机
            //channel.QueueBind(directQueue, directExchange, directKey, null);// Direct模式
            //channel.QueueBind(fanoutQueue1, fanoutExchange, fanoutKey, null);// Fanout模式
            //channel.QueueBind(fanoutQueue2, fanoutExchange, fanoutKey, null);// Fanout模式
            channel.QueueBind(topicQueue, topicExchange, topicKey, null);// Topic模式

            Console.WriteLine("\nRabbitMQ连接成功，请输入消息，输入exit退出！");

            string input;
            do
            {
                input = Console.ReadLine();

                var sendBytes = Encoding.UTF8.GetBytes(input);
                //发布消息
                //channel.BasicPublish("", "hello", null, sendBytes);// hello
                //channel.BasicPublish(directExchange, directKey, null, sendBytes);// Direct模式
                //channel.BasicPublish(fanoutExchange, fanoutKey, null, sendBytes);// Fanout模式
                channel.BasicPublish(topicExchange, "topicKey.one", null, sendBytes);// Topic模式

            } while (input.Trim().ToLower() != "exit");
            channel.Close();
            connection.Close();
        }
    }
}
