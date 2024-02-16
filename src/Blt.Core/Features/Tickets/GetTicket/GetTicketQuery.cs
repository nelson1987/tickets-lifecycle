namespace Blt.Core.Features.Tickets.GetTicket
{
    public record GetTicketQuery
    {
        public Guid Id { get; set; }
        public string Event { get; set; }
        public string Document { get; set; }
    }
}