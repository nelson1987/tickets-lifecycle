namespace Blt.Core
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string Event { get; set; }
    }
    public record CreateTicketCommand
    {
        public string Event { get; set; }
    }
    public record GetTicketQuery
    {
        public Guid Id { get; set; }
    }
}