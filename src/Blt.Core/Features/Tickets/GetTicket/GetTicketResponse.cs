namespace Blt.Core.Features.Tickets.GetTicket;

public record GetTicketResponse
{
    public Guid Id { get; set; }
    public required string Event { get; set; }
    public required string Document { get; set; }
}