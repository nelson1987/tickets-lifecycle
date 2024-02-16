using Blt.Core.Features.Tickets;
using Microsoft.Extensions.DependencyInjection;

namespace Blt.Tests.Configurations;

public class BaseTestFixture : IAsyncDisposable
{
    public readonly ApiFixture Server;

    public readonly ITicketRepository repository;
    public BaseTestFixture()
    {
        Server = new ApiFixture();
        repository = Server.Services.GetRequiredService<ITicketRepository>();
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
}