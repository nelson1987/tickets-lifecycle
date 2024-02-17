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
        await BuyNewTicket(evento, documento);
        await CheckIfNewTicketWasBuyed(evento, documento);
        CheckNewReservedTicketEvent(evento, documento);
    }

    [Fact]
    public async Task BuyTicket_Buy_Fail_Duplicate_Key_IntegrationTest()
    {
        var evento = "Futebol";
        var documento = "12345678901";
        await BuyNewTicket(evento, documento);
        await BuyDuplicateTicket(evento, documento);
    }

    private async Task BuyNewTicket(string @event, string document)
    {
        var command = new BuyTicketCommand()
        {
            Document = document,
            Event = @event
        };

        var response = await Client.PostAsync("/ticket", new StringContent(command.ToJson(), Encoding.UTF8, "application/json"));

        Assert.Equal(201, (int)response.StatusCode);
    }

    private async Task BuyDuplicateTicket(string @event, string document)
    {
        var command = new BuyTicketCommand()
        {
            Document = document,
            Event = @event
        };

        var response = await Client.PostAsync("/ticket", new StringContent(command.ToJson(), Encoding.UTF8, "application/json"));

        Assert.Equal(400, (int)response.StatusCode);
    }

    private async Task CheckIfNewTicketWasBuyed(string @event, string document)
    {
        var response = await Client.GetAsync($"/ticket/{@event}/{document}");

        Assert.Equal(200, (int)response.StatusCode);
    }

    private void CheckNewReservedTicketEvent(string @event, string document)
    {
        var mensagem = MassTransitFixture.TestHarness.Published.PublishedMessage<TicketReservedEvent>();
        Assert.NotNull(mensagem);
        Assert.IsType<TicketReservedEvent>(mensagem.Result);
        Assert.Equal(@event, mensagem.Result.Event);
        Assert.Equal(document, mensagem.Result.Document);
    }
}