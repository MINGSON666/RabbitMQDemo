using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Infrastructure.Extensions
{
    public static class AppBuilderExtension
    {
        public static IApplicationBuilder UseSubscribe(this IApplicationBuilder appBuilder, string subscriptionIdPrefix, Assembly assembly)
        {
            var services = appBuilder.ApplicationServices.CreateScope().ServiceProvider;

            var lifeTime = services.GetService<IHostApplicationLifetime>();
            var bus = services.GetService<IBus>();
            if (lifeTime != null)
            {
                lifeTime.ApplicationStarted.Register(() =>
                {
                    var subscriber = new AutoSubscriber(bus, subscriptionIdPrefix);
                    subscriber.Subscribe(new[] {assembly});
                    subscriber.SubscribeAsync(new[] {typeof(Assembly)});
                });

                lifeTime.ApplicationStopped.Register(() => bus.Dispose());
            }

            return appBuilder;
        }
    }
}
