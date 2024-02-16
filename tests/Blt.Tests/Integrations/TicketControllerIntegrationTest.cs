using Blt.Core;
using Blt.Tests.Configurations;
using System.Text;

namespace Blt.Tests.Integrations
{
    public class TicketControllerIntegrationTest : BaseIntegrationTest
    {
        public TicketControllerIntegrationTest(BaseTestFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task BuyTicket_Buy_Succesfully_IntegrationTest()
        {
            //var newTicket =
            await BuyNewTicket("Futebol", "12345678901");
        }

        private async Task/*<Ticket>*/ BuyNewTicket(string @event, string document)
        {
            var command = new BuyTicketCommand()
            {
                Document = document,
                Event = @event
            };

            var response = await Client.PostAsync("/ticket", new StringContent(command.ToJson(), Encoding.UTF8, "application/json"));

            Assert.Equal(201, (int)response.StatusCode);
        }
    }
}