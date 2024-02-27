using AutoFixture;
using AutoFixture.AutoMoq;
using Blt.Api.Controllers;
using Blt.Core.Features.Tickets;
using Blt.Core.Features.Tickets.BuyTickets;
using Blt.Core.Features.Tickets.GetTicket;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Blt.Tests.Units;

public class BuyTIcketHandlerUnitTest
{
    private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
    private readonly BuyTicketHandler _controller;
    private readonly BuyTicketCommand _request;
    private readonly Ticket _ticket;
    private readonly Mock<ITicketRepository> _repository;
    private readonly Mock<IEventMessaging> _messaging;
    private readonly Guid _gameId;

    public BuyTIcketHandlerUnitTest()
    {
        _request = _fixture.Build<BuyTicketCommand>()
            .Create();

        _repository = _fixture.Freeze<Mock<ITicketRepository>>();
        _repository
             .Setup(x => x.AddTicketAsync(It.IsAny<Ticket>()))
             .Returns(Task.CompletedTask);
        _repository
             .Setup(x => x.GetEventByDocument(_request.Event, _request.Document))
             .Returns(Task.FromResult((Ticket?)null));

        _messaging = _fixture.Freeze<Mock<IEventMessaging>>();
        _messaging
             .Setup(x => x.SendTicketReservedAsync(It.IsAny<TicketReservedEvent>()))
             .Returns(Task.CompletedTask);

        _ticket = _fixture.Build<Ticket>()
            .With(x => x.Event, _request.Event)
            .With(x => x.Document, _request.Document)
            .Create();
        _gameId = Guid.NewGuid();
        _controller = _fixture.Build<BuyTicketHandler>()
            .Create();
    }

    [Fact]
    public async Task BuyTicket_Buy_Succesfully_UnitTest()
    {
        var response = await _controller.Handle(_request, _gameId);
        Assert.True(response.IsSuccess);

        _repository.Verify(x => x.GetEventByDocument(_request.Event, _request.Document), Times.Once);
        _repository.Verify(x => x.AddTicketAsync(It.IsAny<Ticket>()), Times.Once);
        _messaging.Verify(x => x.SendTicketReservedAsync(It.IsAny<TicketReservedEvent>()), Times.Once);
    }

    [Fact]
    public async Task BuyTicket_Document_Has_One_Ticket_Buyed_UnitTest()
    {
        _repository
             .Setup(x => x.GetEventByDocument(_request.Event, _request.Document))
             .Returns(Task.FromResult(_ticket)!);

        var response = await _controller.Handle(_request, _gameId);
        Assert.False(response.IsSuccess);

        _repository.Verify(x => x.GetEventByDocument(_request.Event, _request.Document), Times.Once);
        _repository.Verify(x => x.AddTicketAsync(It.IsAny<Ticket>()), Times.Never);
        _messaging.Verify(x => x.SendTicketReservedAsync(It.IsAny<TicketReservedEvent>()), Times.Never);
    }

    [Fact]
    public async Task BuyTicket_Cant_Add_Ticket_In_Database_UnitTest()
    {
        _repository
             .Setup(x => x.AddTicketAsync(It.IsAny<Ticket>()))
             .Throws(new Exception("Duplicated Key"));

        var response = await _controller.Handle(_request, _gameId);
        Assert.False(response.IsSuccess);

        _repository.Verify(x => x.GetEventByDocument(_request.Event, _request.Document), Times.Once);
        _repository.Verify(x => x.AddTicketAsync(It.IsAny<Ticket>()), Times.Once);
        _messaging.Verify(x => x.SendTicketReservedAsync(It.IsAny<TicketReservedEvent>()), Times.Never);
    }
}
public class TicketControllerUnitTest
{
    private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
    private readonly TicketController _controller;
    private readonly BuyTicketCommand _request;
    private readonly Ticket _ticket;
    private readonly Mock<IValidator<BuyTicketCommand>> _validator;
    private readonly Mock<ITicketRepository> _repository;
    private readonly Mock<IBuyTicketHandler> _messaging;
    private readonly Guid _gameId;

    public TicketControllerUnitTest()
    {
        _request = _fixture.Build<BuyTicketCommand>()
            .Create();

        _validator = _fixture.Freeze<Mock<IValidator<BuyTicketCommand>>>();
        _validator
             .Setup(x => x.Validate(_request))
             .Returns(new FluentValidation.Results.ValidationResult());

        _repository = _fixture.Freeze<Mock<ITicketRepository>>();
        _repository
             .Setup(x => x.AddTicketAsync(It.IsAny<Ticket>()))
             .Returns(Task.CompletedTask);
        _repository
             .Setup(x => x.GetEventByDocument(_request.Event, _request.Document))
             .Returns(Task.FromResult((Ticket?)null));
        _gameId = Guid.NewGuid();
        _messaging = _fixture.Freeze<Mock<IBuyTicketHandler>>();
        _messaging
             .Setup(x => x.Handle(_request, _gameId))
             .Returns(Task.FromResult(Result.Ok()));

        _ticket = _fixture.Build<Ticket>()
            .With(x => x.Event, _request.Event)
            .With(x => x.Document, _request.Document)
            .Create();

        _controller = _fixture.Build<TicketController>()
            .OmitAutoProperties()
            .Create();
    }

    [Fact]
    public async Task BuyTicket_Buy_Succesfully_UnitTest()
    {
        var response = await _controller.Buy(_request);
        ObjectResult result = (ObjectResult)response;
        Assert.Equal(201, result.StatusCode);

        //var responseDto = JsonConvert.DeserializeObject<>(await response.Content.ReadAsStringAsync());

        _validator.Verify(x => x.Validate(_request), Times.Once);
        _messaging.Verify(x => x.Handle(_request, _gameId), Times.Once);
    }

    [Fact]
    public async Task BuyTicket_Invalid_Request_Buyed_UnitTest()
    {
        _validator
                .Setup(x => x.Validate(_request))
                .Returns(new FluentValidation.Results.ValidationResult(new[] { new ValidationFailure("any-prop", "any-error-message") }));

        var response = await _controller.Buy(_request);
        ObjectResult result = (ObjectResult)response;
        Assert.Equal(422, result.StatusCode);

        _validator.Verify(x => x.Validate(_request), Times.Once);
        _messaging.Verify(x => x.Handle(_request, _gameId), Times.Never);
    }

    [Fact]
    public async Task BuyTicket_Document_Fail_Handler_Buyed_UnitTest()
    {
        _messaging
             .Setup(x => x.Handle(_request, _gameId))
             .Returns(Task.FromResult(Result.Fail("error")));

        var response = await _controller.Buy(_request);
        ObjectResult result = (ObjectResult)response;
        Assert.Equal(400, result.StatusCode);

        _validator.Verify(x => x.Validate(_request), Times.Once);
        _messaging.Verify(x => x.Handle(_request, _gameId), Times.Once);
    }

    [Fact]
    public async Task GetTicket_GetById_Succesfully_UnitTest()
    {
        _repository
             .Setup(x => x.GetEventByDocument(_request.Event, _request.Document))
             .Returns(Task.FromResult(_ticket)!);

        var response = await _controller.GetById(_request.Event, _request.Document);
        ObjectResult responseResult = (ObjectResult)response.Result!;
        Assert.Equal(200, responseResult.StatusCode);

        GetTicketResponse responseValue = (GetTicketResponse)responseResult.Value!;
        Assert.NotNull(responseResult.Value);
        Assert.Equal(_request.Event, responseValue.Event);
        Assert.Equal(_request.Document, responseValue.Document);

        _repository.Verify(x => x.GetEventByDocument(_request.Event, _request.Document), Times.Once);
    }
}