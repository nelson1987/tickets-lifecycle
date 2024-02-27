using Blt.Core.Consumers;
using Blt.Core.Features.Tickets;
using Blt.Core.Features.Tickets.BuyTickets;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Blt.Core;

public static class Dependencies
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IBuyTicketHandler, BuyTicketHandler>()
                .AddScoped<IValidator<BuyTicketCommand>, BuyTicketCommandValidator>()
                .AddScoped<ITicketRepository, TicketRepository>()
                .AddScoped<IMatchRepository, MatchRepository>()
                .AddScoped<IEventMessaging, EventMessaging>();
        return services;
    }
    public static IServiceCollection AddMessaging(this IServiceCollection services)
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

        return services;
    }
}