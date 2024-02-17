using FluentValidation;

namespace Blt.Core.Features.Tickets.BuyTickets;
public class BuyTicketCommandValidator : AbstractValidator<BuyTicketCommand>
{
    public BuyTicketCommandValidator()
    {
        RuleFor(x => x.Event).NotNull().NotEmpty();
        RuleFor(x => x.Document).NotNull().NotEmpty();
    }
}
