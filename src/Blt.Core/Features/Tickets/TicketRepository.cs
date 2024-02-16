namespace Blt.Core.Features.Tickets
{
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
}