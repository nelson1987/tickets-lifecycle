namespace Blt.Core.Features.Tickets.GetTicket
{
    public record GetTicketResponse
    {
        public Guid Id { get; set; }
        public string Event { get; set; }
        public string Document { get; set; }
    }
}