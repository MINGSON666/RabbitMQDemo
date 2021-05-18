using EasyNetQ.AutoSubscribe;
using Models.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZAPEngineService.Services
{
    public class ClientMessageConsumer : IConsumeAsync<ClientMessage>
    {
        [AutoSubscriberConsumer(SubscriptionId = "ClientMessageService.ZapQuestion")]
        public Task ConsumeAsync(ClientMessage message)
        {
            return Task.CompletedTask;
        }

        public Task ConsumeAsync(ClientMessage message, CancellationToken cancellationToken = new CancellationToken())
        {
            // Your business logic code here
            // eg.Generate one ZAP question records into database and send to client
            // Sample console code
            System.Console.ForegroundColor = System.ConsoleColor.Red;
            System.Console.WriteLine("Consume one message from RabbitMQ : {0}, I will generate one ZAP question list to client", message.ClientName);
            System.Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}
