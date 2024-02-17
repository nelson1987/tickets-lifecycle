namespace Blt.Tests.Configurations;

[Collection("Base Collection")]
public abstract class BaseIntegrationTest
{
    protected BaseTestFixture Fixture { get; }
    protected readonly HttpClient Client;
    protected readonly ApiFixture Server;
    protected readonly MongoFixture MongoFixture;
    protected readonly MassTransitFixture MassTransitFixture;

    protected BaseIntegrationTest(BaseTestFixture fixture)
    {
        Fixture = fixture;
        MongoFixture = fixture.MongoFixture;
        MassTransitFixture = fixture.MassTransitFixture;
        Server = fixture.Server;
        Client = Server.CreateClient();

        Fixture.Reset().GetAwaiter();
    }
}