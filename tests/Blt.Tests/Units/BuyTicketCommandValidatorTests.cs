using AutoFixture;
using AutoFixture.AutoMoq;
using Blt.Core.Features.Tickets.BuyTickets;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Blt.Tests.Units;
public class BuyTicketCommandValidatorTests
{
    private readonly IFixture _fixture = new Fixture().Customize(new AutoMoqCustomization());
    private readonly IValidator<BuyTicketCommand> _validator;
    private readonly BuyTicketCommand _command;

    public BuyTicketCommandValidatorTests()
    {
        _command = _fixture.Build<BuyTicketCommand>()
            .Create();
        _validator = _fixture.Create<BuyTicketCommandValidator>();
    }

    [Fact]
    public void Given_a_valid_command_when_all_fields_are_valid_should_pass_validation()
        => _validator
            .TestValidate(_command)
            .ShouldNotHaveAnyValidationErrors();

    [Fact]
    public void Given_a_request_with_invalid_event_should_fail_validation()
        => _validator
            .TestValidate(_command with { Event = string.Empty })
            .ShouldHaveValidationErrorFor(x => x.Event)
            .Only();

    [Fact]
    public void Given_a_request_with_invalid_document_should_fail_validation()
        => _validator
            .TestValidate(_command with { Document = string.Empty })
            .ShouldHaveValidationErrorFor(x => x.Document)
            .Only();
}