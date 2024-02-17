using Blt.Core.Features.Tickets.BuyTickets;
using MassTransit;

namespace Blt.Core.Features.Tickets;

public class EventMessaging : IEventMessaging
{
    private readonly IBus _bus;

    public EventMessaging(IBus bus)
    {
        _bus = bus;
    }

    public async Task SendTicketReservedAsync(TicketReservedEvent @event)
    {
        await _bus.Publish(@event);
        //await Task.CompletedTask;
    }
}