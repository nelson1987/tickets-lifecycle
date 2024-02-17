using Blt.Core.Features.Tickets;
using Microsoft.Extensions.DependencyInjection;

namespace Blt.Tests.Configurations;

public class MongoFixture
{
    public readonly ITicketRepository repository;
    public MongoFixture(ApiFixture Server)
    {
        repository = Server.Services.GetRequiredService<ITicketRepository>();
    }
    public async Task DeleteAll()
    {
        await repository.DeleteAll();
    }
}
