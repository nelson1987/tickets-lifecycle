namespace Blt.Core.Features.Tickets
{
    public interface ITicketRepository
    {
        Task AddTicketAsync(Ticket ticket);

        Task<Ticket?> GetEventByDocument(string @event, string document);
    }
}