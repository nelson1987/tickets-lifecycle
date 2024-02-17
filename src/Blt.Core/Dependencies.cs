using Blt.Core.Consumers;
using Blt.Core.Features.Tickets;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Blt.Core;

public static class Dependencies
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<ITicketRepository, TicketRepository>()
                .AddScoped<IEventMessaging, EventMessaging>();
        return services;
    }
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumer<TicketReservedConsumer>();
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("amqp://guest:guest@localhost:5672");
                    cfg.ConfigureEndpoints(ctx);
                });
            });
        });
        return services;
    }
}