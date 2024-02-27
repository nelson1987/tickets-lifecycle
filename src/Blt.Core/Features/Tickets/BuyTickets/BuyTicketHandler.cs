using Blt.Core.Utils;
using FluentResults;

namespace Blt.Core.Features.Tickets.BuyTickets;
public interface IBuyTicketHandler
{
    Task<Result> Handle(BuyTicketCommand command, Guid idGame);
}
public class BuyTicketHandler : IBuyTicketHandler
{
    private readonly ITicketRepository _repository;
    private readonly IEventMessaging _eventMessaging;

    public BuyTicketHandler(ITicketRepository repository,
        IEventMessaging eventMessaging)
    {
        _repository = repository;
        _eventMessaging = eventMessaging;
    }
    public async Task<Result> Handle(BuyTicketCommand command, Guid idGame)
    {
        try
        {
            var purchasedTicket = await _repository.GetEventByDocument(command.Event, command.Document);
            if (purchasedTicket != null)
                return Result.Fail($"Já existe ticket para {command.Event} comprado com este documento.");

            var purchasableticket = command.MapTo<Ticket>();
            purchasableticket.Open();
            await _repository.AddTicketAsync(purchasableticket);

            var reservedticket = purchasableticket.MapTo<TicketReservedEvent>();
            reservedticket.GameId = idGame;
            await _eventMessaging.SendTicketReservedAsync(reservedticket);

            return Result.Ok();
        }
        catch (Exception)
        {
            return Result.Fail("Erro ao tentar comprar um ingresso.");
        }
    }
}
