using DemandManagement.MessageContracts;
using GreenPipes;
using MassTransit;
using System;

namespace DemandManagement.Notification
{
    class Program
    {
        static  void Main(string[] args)
        {
            Console.Title = "Notification";
            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint(RabbitMqConsts.NotificationServiceQueue, e =>
                {
                    e.Consumer<DemandRegisteredEventConsumer>();
                    // 它将尝试进行5次相关操作（消息）。如果错误仍然存​​在，它将把消息发送到错误队列并继续进行下一个操作
                    e.UseMessageRetry(r => r.Immediate(5));
                });
            });

            bus.StartAsync();
            Console.WriteLine("Listening for Demand registered events.. Press enter to exit");
            Console.ReadLine();
            bus.StopAsync();

        }
    }
}
