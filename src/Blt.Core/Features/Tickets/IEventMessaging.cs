using Blt.Core.Features.Tickets.BuyTickets;

namespace Blt.Core.Features.Tickets;

public interface IEventMessaging
{
    Task SendTicketReservedAsync(TicketReservedEvent @event);
}