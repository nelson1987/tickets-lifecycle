namespace Blt.Core.Features.Tickets.BuyTickets;

public record TicketReservedEvent
{
    public Guid Id { get; set; }
    public required string Event { get; set; }
    public required string Document { get; set; }
    public Guid GameId { get; set; }
}