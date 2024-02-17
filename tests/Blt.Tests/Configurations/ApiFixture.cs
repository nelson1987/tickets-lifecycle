using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Blt.Tests.Configurations;

public class ApiFixture : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
        => builder.UseEnvironment("Testing")
                    .ConfigureServices(services => services.AddMassTransitTestHarness());
}