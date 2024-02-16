namespace Blt.Core.Features.Tickets.BuyTickets;

public record BuyTicketCommand
{
    public required string Event { get; set; }
    public required string Document { get; set; }
}