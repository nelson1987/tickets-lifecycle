using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Blt.Tests.Configurations
{
    public class BaseIntegrationTest
    {
    }

    [CollectionDefinition("Base Collection")]
    public class BaseTestCollection : ICollectionFixture<BaseTestFixture>
    {
    }

    public class BaseTestFixture : IAsyncDisposable
    {
        public readonly HttpClient HttpClient;
        public readonly TestServer Server;
        //public readonly MainContext MainContext;
        public BaseTestFixture()
        {
            Server = new TestServer(
                WebHost
                .CreateDefaultBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Program>());
            HttpClient = Server.CreateClient();
        }
        public async ValueTask DisposeAsync()
        {
            HttpClient.Dispose();
            Server.Dispose();
            await Task.CompletedTask;
        }
    }
}
