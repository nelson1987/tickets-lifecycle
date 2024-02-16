using AutoFixture;
using AutoFixture.AutoMoq;
using Blt.Api.Controllers;
using Blt.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using System.Text;

namespace Blt.Tests.Configurations
{
    public class ApiFixture : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        public readonly HttpClient Client;

        public ApiFixture(WebApplicationFactory<Program> webApplicationfactory)
        {

            var _factory = webApplicationfactory
                .WithWebHostBuilder(builder => {
                    builder.UseEnvironment("Testing");
                    builder.ConfigureServices(services => {
                        //services.Add
                    });
                });
            Client = _factory.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }


    [CollectionDefinition("Base Collection")]
    public class BaseTestCollection : ICollectionFixture<BaseTestFixture>
    {
    }

    [Collection("Base Collection")]
    public abstract class BaseIntegrationTest
    {
        protected BaseTestFixture Fixture { get; }
        //protected readonly HttpClient Client;
        protected readonly ApiFixture Server;

        protected BaseIntegrationTest(BaseTestFixture fixture)
        {
            Fixture = fixture;
            //Client = fixture.Client;
            Server = fixture.Server;
        }
    }

    public class BaseTestFixture : IAsyncDisposable
    {
        //public readonly HttpClient Client;
        public readonly ApiFixture Server;
        //public readonly WebApplicationFactory<Program> _factory;
        //public readonly MainContext MainContext;
        public BaseTestFixture()
        {
            //Client = Server.CreateClient();
        }
        public async ValueTask DisposeAsync()
        {
            //Client.Dispose();
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

            var response = await Server.Client.PostAsync("ticket", new StringContent(command.ToJson(), Encoding.UTF8, "application/json"));

            Assert.Equal(201, (int)response.StatusCode);

            //throw new NotImplementedException();
        }
        /*
// Serialize our concrete class into a JSON String
var stringPayload = JsonConvert.SerializeObject(payload);

// Wrap our JSON inside a StringContent which then can be used by the HttpClient class
var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

var httpClient = new HttpClient()
    
// Do the actual request and await the response
var httpResponse = await httpClient.PostAsync("http://localhost/api/path", httpContent);

// If the response contains content we want to read it!
if (httpResponse.Content != null) {
    var responseContent = await httpResponse.Content.ReadAsStringAsync();
    
    // From here on you could deserialize the ResponseContent back again to a concrete C# type using Json.Net
}
         */


    }
    public class TicketControllerUnitTest
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
        private readonly TicketController _controller;
        private readonly BuyTicketCommand _command;
        private readonly Mock<ITicketRepository> _repository;
        private readonly Mock<IEventMessaging> _messaging;

        public TicketControllerUnitTest()
        {
            _command = _fixture.Build<BuyTicketCommand>()
                .Create();

            _repository = _fixture.Freeze<Mock<ITicketRepository>>();
            _repository
                 .Setup(x => x.AddTicketAsync(It.IsAny<Ticket>()))
                 .Returns(Task.CompletedTask);
            _repository
                 .Setup(x => x.GetEventByDocument(_command.Event, _command.Document))
                 .Returns(Task.FromResult((Ticket)null));

            _messaging = _fixture.Freeze<Mock<IEventMessaging>>();
            _messaging
                 .Setup(x => x.SendTicketReservedAsync(It.IsAny<TicketReservedEvent>()))
                 .Returns(Task.CompletedTask);

            _controller = _fixture.Build<TicketController>()
                .OmitAutoProperties()
                .Create();
        }

        [Fact]
        public async Task BuyTicket_Buy_Succesfully_UnitTest()
        {
            var response = await _controller.Buy(_command);
            ObjectResult result = (ObjectResult)response;
            Assert.Equal(201, result.StatusCode);

            //var responseDto = JsonConvert.DeserializeObject<>(await response.Content.ReadAsStringAsync());

            _repository.Verify(x => x.GetEventByDocument(_command.Event, _command.Document), Times.Once);
            _repository.Verify(x => x.AddTicketAsync(It.IsAny<Ticket>()), Times.Once);
            _messaging.Verify(x => x.SendTicketReservedAsync(It.IsAny<TicketReservedEvent>()), Times.Once);
        }

        [Fact]
        public async Task BuyTicket_Document_Has_One_Ticket_Buyed_UnitTest()
        {
            _repository
                 .Setup(x => x.GetEventByDocument(_command.Event, _command.Document))
                 .Returns(Task.FromResult(new Ticket()));

            var response = await _controller.Buy(_command);
            ObjectResult result = (ObjectResult)response;
            Assert.Equal(400, result.StatusCode);

            _repository.Verify(x => x.GetEventByDocument(_command.Event, _command.Document), Times.Once);
            _repository.Verify(x => x.AddTicketAsync(It.IsAny<Ticket>()), Times.Never);
            _messaging.Verify(x => x.SendTicketReservedAsync(It.IsAny<TicketReservedEvent>()), Times.Never);
        }

        [Fact]
        public async Task BuyTicket_Cant_Add_Ticket_In_Database_UnitTest()
        {
            _repository
                 .Setup(x => x.AddTicketAsync(It.IsAny<Ticket>()))
                 .Throws(new Exception("Duplicated Key"));

            var response = await _controller.Buy(_command);
            ObjectResult result = (ObjectResult)response;
            Assert.Equal(400, result.StatusCode);

            _repository.Verify(x => x.GetEventByDocument(_command.Event, _command.Document), Times.Once);
            _repository.Verify(x => x.AddTicketAsync(It.IsAny<Ticket>()), Times.Once);
            _messaging.Verify(x => x.SendTicketReservedAsync(It.IsAny<TicketReservedEvent>()), Times.Never);
        }
    }
}
