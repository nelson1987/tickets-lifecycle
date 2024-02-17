namespace Blt.Tests.Configurations;

public class BaseTestFixture : IAsyncDisposable
{
    public readonly ApiFixture Server;
    public readonly MongoFixture MongoFixture;
    public readonly MassTransitFixture MassTransitFixture;
    public BaseTestFixture()
    {
        Server = new ApiFixture();
        MongoFixture = new MongoFixture(Server);
        MassTransitFixture = new MassTransitFixture(Server);
    }

    public async ValueTask DisposeAsync()
    {
        Server.Dispose();
        await Task.CompletedTask;
    }

    public async Task Reset()
    {
        await MongoFixture.DeleteAll();
        await MassTransitFixture.ConsumeAll();
    }
}