namespace Blt.Core.Features.Tickets.BuyTickets
{
    public record TicketReservedEvent
    {
        public Guid Id { get; set; }
        public string Event { get; set; }
        public string Document { get; set; }
    }
}