using AutoFixture;
using AutoFixture.AutoMoq;
using Blt.Core.Consumers;
using Blt.Core.Features.Tickets.BuyTickets;
using MassTransit;

namespace Blt.Tests.Integrations;

public class TicketReservedConsumerUnitTest
{
    private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
    private readonly TicketReservedConsumer consumer;
    private readonly TicketReservedEvent @event;
    private readonly ConsumeContext<TicketReservedEvent> context;
    public TicketReservedConsumerUnitTest()
    {
        /*
        _request = _fixture.Build<BuyTicketCommand>()
            .Create();

        _validator = _fixture.Freeze<Mock<IValidator<BuyTicketCommand>>>();
        _validator
             .Setup(x => x.Validate(_request))
             .Returns(new FluentValidation.Results.ValidationResult());
        */
        consumer = _fixture.Build<TicketReservedConsumer>()
                    .Create();
        @event = _fixture.Build<TicketReservedEvent>()
                    .Create();
        context = _fixture.Build<ConsumeContext<TicketReservedEvent>>()
                    .With(x => x.Message, @event)
                    .Create();
    }

    [Fact]
    public async Task BuyTicket_Buy_Succesfully_UnitTest()
    {
        await consumer.Consume(context);

        //   _validator.Verify(x => x.Validate(_request), Times.Once);
        //   _repository.Verify(x => x.GetEventByDocument(_request.Event, _request.Document), Times.Once);
        //   _repository.Verify(x => x.AddTicketAsync(It.IsAny<Ticket>()), Times.Once);
        //   _messaging.Verify(x => x.SendTicketReservedAsync(It.IsAny<TicketReservedEvent>()), Times.Once);
    }
}
//TODO: Criar IntegrationTests
public class TicketReservedConsumerIntegrationTest
{

}
