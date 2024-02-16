using Blt.Core.Features.Tickets.BuyTickets;

namespace Blt.Core.Features.Tickets
{
    public class EventMessaging : IEventMessaging
    {
        public async Task SendTicketReservedAsync(TicketReservedEvent @event)
        {
            await Task.CompletedTask;
        }
    }
}