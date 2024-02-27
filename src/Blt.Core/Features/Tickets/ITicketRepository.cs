using Blt.Core.Features.FanMember;

namespace Blt.Core.Features.Tickets;

public interface ITicketRepository
{
    Task AddTicketAsync(Ticket ticket);
    Task<Ticket?> GetEventByDocument(string @event, string document);
    Task<long> DeleteAll();
    Task EditAsync(Ticket ticket);
}
public interface IMatchRepository
{
    Task AddMatchAsync(Game Match);
    //Task<Match?> GetMatchByDocument(string data);
}