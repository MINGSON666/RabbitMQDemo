using DemandManagement.MessageContracts;
using GreenPipes;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DemandManagement.Registration
{
    public class RegisterDemandCommandConsumer : IConsumer<IRegisterDemandCommand>
    {
        public Task Consume(ConsumeContext<IRegisterDemandCommand> context)
        {
            //// 在默认的总线配置下，异常由传输中的中间件捕获（ErrorTransportFilter确切地说是），并且消息被移至_error队列（由接收端点队列名称作为前缀）
            //throw new Exception("Very bad things happened");

            var message = context.Message;
            var guid = Guid.NewGuid();
            Console.WriteLine($"Demand successfully  registered. Subject:{message.Subject}, Description:{message.Description}, Id:{guid}");
            context.Publish<IRegisteredDemandEvent>(new
            {
                DemandId = guid
            });
            return Task.CompletedTask;
        }
    }
}
