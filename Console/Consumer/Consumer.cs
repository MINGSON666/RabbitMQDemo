using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    class Consumer
    {
        static void Main(string[] args)
        {
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
            var channel2 = connection.CreateModel();

            //事件基本消费者
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            EventingBasicConsumer consumer2 = new EventingBasicConsumer(channel2);

            //接收到消息事件
            consumer.Received += (ch, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"收到消息： {message}");

                //Console.WriteLine($"收到该消息[{ea.DeliveryTag}] 延迟2s发送回执");
                //Thread.Sleep(2000);
                //确认该消息已被消费
                channel.BasicAck(ea.DeliveryTag, false);
                Console.WriteLine($"已发送回执[{ea.DeliveryTag}]");
            };
            consumer2.Received += (ch, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"收到消息： {message}");

                //确认该消息已被消费
                channel.BasicAck(ea.DeliveryTag, false);
                Console.WriteLine($"已发送回执[{ea.DeliveryTag}]");
            };

            //启动消费者 设置为手动应答消息
            //channel.BasicConsume("hello", false, consumer);// hello
            //channel.BasicConsume("directQueue", false, consumer);// Direct模式
            channel.BasicConsume("fanoutQueue1", false, consumer);// Fanout模式
            channel.BasicConsume("fanoutQueue2", false, consumer2);// Fanout模式
            //channel.BasicConsume("topicQueue", false, consumer);// Topic模式

            Console.WriteLine("消费者已启动");
            Console.ReadKey();
            channel.Dispose();
            connection.Close();
		}
    }
}
