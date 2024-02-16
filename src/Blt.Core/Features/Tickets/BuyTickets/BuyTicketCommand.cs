namespace Blt.Core.Features.Tickets.BuyTickets
{
    public record BuyTicketCommand
    {
        public string Event { get; set; }
        public string Document { get; set; }
    }
}