namespace Blt.Core.Features.Tickets.GetTicket;

public record GetTicketQuery
{
    public Guid Id { get; set; }
    public required string Event { get; set; }
    public required string Document { get; set; }
}