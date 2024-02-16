namespace Blt.Core.Features.Tickets;

public class Ticket
{
    public Guid Id { get; set; }
    public required string Event { get; set; }
    public required string Document { get; set; }
}