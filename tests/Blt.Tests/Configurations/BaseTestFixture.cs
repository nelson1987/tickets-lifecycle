using Blt.Core.Consumers;
using Blt.Core.Features.Tickets;
using Blt.Core.Features.Tickets.BuyTickets;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Blt.Tests.Configurations;

public class BaseTestFixture : IAsyncDisposable
{
    public readonly ApiFixture Server;
    public readonly ITicketRepository repository;
    public readonly ITestHarness TestHarness;
    public readonly TicketReservedConsumer Consumer;
    public BaseTestFixture()
    {
        Server = new ApiFixture();
        repository = Server.Services.GetRequiredService<ITicketRepository>();
        TestHarness = Server.Services.GetTestHarness();//GetRequiredService<IConsumer<TicketReservedEvent>>();
        Consumer = Server.Services.GetRequiredService<TicketReservedConsumer>();
    }

    public async ValueTask DisposeAsync()
    {
        Server.Dispose();
        await Task.CompletedTask;
    }

    public async Task DeleteAll()
    {
        await repository.DeleteAll();
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