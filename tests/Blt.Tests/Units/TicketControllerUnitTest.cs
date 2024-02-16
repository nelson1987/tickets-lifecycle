using AutoFixture;
using AutoFixture.AutoMoq;
using Blt.Api.Controllers;
using Blt.Core.Features.Tickets;
using Blt.Core.Features.Tickets.BuyTickets;
using Blt.Core.Features.Tickets.GetTicket;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;

namespace Blt.Tests.Units
{
    public class TicketControllerUnitTest
    {
        private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
        private readonly TicketController _controller;
        private readonly BuyTicketCommand _command;
        private readonly Ticket _ticket;
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

            _ticket = _fixture.Build<Ticket>()
                .With(x => x.Event, _command.Event)
                .With(x => x.Document, _command.Document)
                .Create();

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
                 .Returns(Task.FromResult(_ticket)!);

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


        [Fact]
        public async Task GetTicket_GetById_Succesfully_UnitTest()
        {
            _repository
                 .Setup(x => x.GetEventByDocument(_command.Event, _command.Document))
                 .Returns(Task.FromResult(_ticket)!);

            var response = await _controller.GetById(_command.Event, _command.Document);
            ObjectResult responseResult = (ObjectResult)response.Result!;
            Assert.Equal(200, responseResult.StatusCode);

            GetTicketResponse responseValue = (GetTicketResponse)responseResult.Value!;
            Assert.NotNull(responseResult.Value);
            Assert.Equal(_command.Event, responseValue.Event);
            Assert.Equal(_command.Document, responseValue.Document);

            _repository.Verify(x => x.GetEventByDocument(_command.Event, _command.Document), Times.Once);
        }

    }
}