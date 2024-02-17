using Blt.Core.Features.Tickets.BuyTickets;
using MassTransit;

namespace Blt.Core.Consumers;
public class TicketReservedConsumer : IConsumer<TicketReservedEvent>
{
    public Task Consume(ConsumeContext<TicketReservedEvent> context)
    {
        //throw new NotImplementedException();
        return Task.CompletedTask;
    }
}
