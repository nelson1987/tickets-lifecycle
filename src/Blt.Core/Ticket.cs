namespace Blt.Core
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string Event { get; set; }
        public string Document { get; set; }
    }

    public record BuyTicketCommand
    {
        public string Event { get; set; }
        public string Document { get; set; }
    }
    public record GetTicketQuery
    {
        public Guid Id { get; set; }
    }
    public record GetTicketResponse
    {
        public Guid Id { get; set; }
        public string Event { get; set; }
        public string Document { get; set; }
    }
    public record TicketReservedEvent
    {
        public Guid Id { get; set; }
        public string Event { get; set; }
        public string Document { get; set; }
    }

    public interface ITicketRepository
    {
        Task AddTicketAsync(Ticket ticket);

        Task<Ticket?> GetEventByDocument(string @event, string document);
    }

    public interface IEventMessaging
    {
        Task SendTicketReservedAsync(TicketReservedEvent @event);
    }

    public class TicketRepository : ITicketRepository
    {
        public async Task AddTicketAsync(Ticket ticket)
        {
            await Task.CompletedTask;
        }

        public Task<Ticket?> GetEventByDocument(string @event, string document)
        {
            return Task.FromResult((Ticket)null);
        }
    }

    public class EventMessaging : IEventMessaging
    {
        public async Task SendTicketReservedAsync(TicketReservedEvent @event)
        {
            await Task.CompletedTask;
        }
    }
}