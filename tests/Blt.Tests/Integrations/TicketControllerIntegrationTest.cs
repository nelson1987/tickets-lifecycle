using Blt.Core.Features.Tickets.BuyTickets;
using Blt.Core.Utils;
using Blt.Tests.Configurations;
using System.Text;

namespace Blt.Tests.Integrations;

public class TicketControllerIntegrationTest : BaseIntegrationTest
{
    public TicketControllerIntegrationTest(BaseTestFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task BuyTicket_Buy_Succesfully_IntegrationTest()
    {
        var evento = "Futebol";
        var documento = "12345678901";
        var reserved = await BuyNewTicket(evento, documento);
        Assert.True(reserved);
        await CheckIfNewTicketWasBuyed(evento, documento);
    }

    [Fact]
    public async Task BuyTicket_Buy_Fail_Duplicate_Key_IntegrationTest()
    {
        var evento = "Futebol";
        var documento = "12345678901";
        var reserved = await BuyNewTicket(evento, documento);
        Assert.True(reserved);
        var reserved2 = await BuyDuplicateTicket(evento, documento);
        Assert.True(reserved2);
    }

    private async Task<bool> BuyNewTicket(string @event, string document)
    {
        var command = new BuyTicketCommand()
        {
            Document = document,
            Event = @event
        };

        var response = await Client.PostAsync("/ticket", new StringContent(command.ToJson(), Encoding.UTF8, "application/json"));

        Assert.Equal(201, (int)response.StatusCode);

        return (int)response.StatusCode == 201;
    }
    private async Task<bool> BuyDuplicateTicket(string @event, string document)
    {
        var command = new BuyTicketCommand()
        {
            Document = document,
            Event = @event
        };

        var response = await Client.PostAsync("/ticket", new StringContent(command.ToJson(), Encoding.UTF8, "application/json"));

        Assert.Equal(400, (int)response.StatusCode);

        return (int)response.StatusCode == 400;
    }

    private async Task CheckIfNewTicketWasBuyed(string @event, string document)
    {
        var response = await Client.GetAsync($"/ticket/{@event}/{document}");

        Assert.Equal(200, (int)response.StatusCode);

    }
}