using Blt.Core.Features.Tickets;
using Microsoft.Extensions.DependencyInjection;

namespace Blt.Core
{
    public static class Dependencies
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddScoped<ITicketRepository, TicketRepository>()
                    .AddScoped<IEventMessaging, EventMessaging>();
            return services;
        }
    }
}