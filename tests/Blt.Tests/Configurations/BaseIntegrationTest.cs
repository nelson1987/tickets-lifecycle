namespace Blt.Tests.Configurations;

[Collection("Base Collection")]
public abstract class BaseIntegrationTest
{
    protected BaseTestFixture Fixture { get; }
    protected readonly HttpClient Client;

    //protected readonly HttpClient Client;
    protected readonly ApiFixture Server;

    protected BaseIntegrationTest(BaseTestFixture fixture)
    {
        Fixture = fixture;
        //Client = fixture.Client;
        Server = fixture.Server;
        Client = Server.CreateClient();

        Fixture.DeleteAll().GetAwaiter();
        Fixture.ConsumeAll().GetAwaiter();
    }
}