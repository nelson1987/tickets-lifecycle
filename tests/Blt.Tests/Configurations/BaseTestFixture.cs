namespace Blt.Tests.Configurations
{
    public class BaseTestFixture : IAsyncDisposable
    {
        public readonly ApiFixture Server;

        //public readonly MainContext MainContext;
        public BaseTestFixture()
        {
            Server = new ApiFixture();
        }

        public async ValueTask DisposeAsync()
        {
            Server.Dispose();
            await Task.CompletedTask;
        }
    }
}