namespace Blt.Core.Features.Tickets;

public class Ticket
{
    public Guid Id { get; set; }
    public required string Event { get; set; }
    public required string Document { get; set; }
    public required TicketStatus Status { get; set; }

    public void Open()
    {
        Status = TicketStatus.Open;
    }
}
public enum TicketStatus { Open = 1, Reserved = 2, Selled = 3, Cancelled = 99 }