using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Blt.Tests.Configurations
{

    [CollectionDefinition("Base Collection")]
    public class BaseTestCollection : ICollectionFixture<BaseTestFixture>
    {
    }

    [Collection("Base Collection")]
    public abstract class BaseIntegrationTest
    {
        protected BaseTestFixture Fixture { get; }
        protected readonly HttpClient Client;
        protected readonly TestServer Server;

        protected BaseIntegrationTest(BaseTestFixture fixture)
        {
            Fixture = fixture;
            Client = fixture.Client;
            Server = fixture.Server;
        }
    }

    public class BaseTestFixture : IAsyncDisposable
    {
        public readonly HttpClient Client;
        public readonly TestServer Server;
        //public readonly MainContext MainContext;
        public BaseTestFixture()
        {
            Server = new TestServer(
                WebHost
                .CreateDefaultBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Program>());
            Client = Server.CreateClient();
        }
        public async ValueTask DisposeAsync()
        {
            Client.Dispose();
            Server.Dispose();
            await Task.CompletedTask;
        }
    }

    public class TicketControllerIntegrationTest : BaseIntegrationTest 
    {
        public TicketControllerIntegrationTest(BaseTestFixture fixture) : base(fixture)
        {
            
        }
        [Fact]
        public async Task GetTickets_GetById_IntegrationTest() 
        { 
        }
    }
}
