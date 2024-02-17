using Blt.Core.Consumers;
using Blt.Core.Features.Tickets.BuyTickets;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Blt.Tests.Configurations;

public class MassTransitFixture
{
    public readonly ITestHarness TestHarness;
    public readonly TicketReservedConsumer Consumer;

    public MassTransitFixture(ApiFixture Server)
    {
        TestHarness = Server.Services.GetTestHarness();
        Consumer = Server.Services.GetRequiredService<TicketReservedConsumer>();
    }

    public async Task ConsumeAll()
    {
        var sagatestharness = TestHarness.GetConsumerHarness<TicketReservedConsumer>();

        var list = sagatestharness.Consumed
            .Select<TicketReservedEvent>()
            .ToArray();
        foreach (var item in list)
        {
            await Consumer.Consume(item.Context);
        }
    }
}
