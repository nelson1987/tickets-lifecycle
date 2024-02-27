using Blt.Core.Features.Tickets;
using Microsoft.Extensions.DependencyInjection;

namespace Blt.Tests.Configurations;

public class MongoFixture
{
    public readonly ITicketRepository ticketRepository;
    public MongoFixture(ApiFixture Server)
    {
        ticketRepository = Server.Services.GetRequiredService<ITicketRepository>();
    }
    public async Task DeleteAll()
    {
        await ticketRepository.DeleteAll();
    }
}
