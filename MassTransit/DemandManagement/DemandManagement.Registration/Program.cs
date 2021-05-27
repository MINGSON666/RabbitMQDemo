using DemandManagement.MessageContracts;
using GreenPipes;
using MassTransit;
using System;
using System.Data;
using MassTransit.Pipeline.Filters;

namespace DemandManagement.Registration
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Registration";

            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint(
                    RabbitMqConsts.RegisterDemandServiceQueue, e =>
                    {
                        e.Consumer<RegisterDemandCommandConsumer>();
                        // 断路器：断路器用于保护处于故障状态的资源（远程，本地或其他）不会过载
                        e.UseCircuitBreaker(cb =>
                        {
                            cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                            cb.TripThreshold = 15;
                            cb.ActiveThreshold = 10;
                            cb.ResetInterval = TimeSpan.FromMinutes(5);
                        });

                        //// 为多种异常类型指定过滤器
                        //e.UseMessageRetry(r =>
                        //{
                        //    r.Handle<ArgumentNullException>();
                        //    r.Ignore(typeof(InvalidOperationException), typeof(InvalidCastException));
                        //    r.Ignore<ArgumentException>(t => t.ParamName == "orderTotal");
                        //});

                        //// 为单个端点指定多个重试策略
                        //e.UseMessageRetry(r =>
                        //{
                        //    r.Immediate(5);
                        //    r.Handle<DataException>(x => x.Message.Contains("SQL"));
                        //});
                        //e.Consumer<RegisterDemandCommandConsumer>(c => c.UseMessageRetry(r =>
                        //    {
                        //        r.Interval(10, TimeSpan.FromMilliseconds(200));
                        //        r.Ignore<ArgumentNullException>();
                        //        r.Ignore<DataException>(x => x.Message.Contains("SQL"));
                        //    })
                        //);

                        //// 如果前5次立即重试失败（数据库确实非常崩溃），则消息将在5、15和30分钟后再重试3次
                        //e.UseScheduledRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30)));
                        //e.UseMessageRetry(r => r.Immediate(5));
                        //// MassTransit提供了一个发件箱来缓冲这些消息
                        //e.UseInMemoryOutbox();

                        //// 错误管道默认过滤器
                        //e.ConfigureError(x =>
                        //{
                        //    x.UseFilter(new GenerateFaultFilter());
                        //    x.UseFilter(new ErrorTransportFilter());
                        //});

                        //// 死信管道默认过滤器
                        //e.ConfigureDeadLetter(x =>
                        //{
                        //    x.UseFilter(new DeadLetterTransportFilter());
                        //});
                    });
            });

            bus.StartAsync();

            Console.WriteLine("Listening for Register Demand commands.. " +
                              "Press enter to exit");
            Console.ReadLine();

            bus.StopAsync();
        }
    }
}
