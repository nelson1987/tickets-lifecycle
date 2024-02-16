using AutoFixture;
using AutoFixture.AutoMoq;
using Blt.Api.Controllers;
using Blt.Core;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Blt.Tests.Units
{
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