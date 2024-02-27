using Blt.Core.Utils;
using FluentResults;

namespace Blt.Core.Features.FanMember
{
    public class Game
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public GameStatus Status { get; set; }
        public int Capacity { get; set; }
        public int AvailableTickets { get; set; }
    }
    public enum GameStatus
    {
        Available = 1, Cancel = 2, SoldOut = 3
    }
    public class FanMember
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
        public decimal Amount { get; set; }
        public FanMemberStatus Status { get; set; }
        public void Open()
        {
            Status = FanMemberStatus.WaitingApprove;
        }
    }
    public enum FanMemberStatus
    {
        Available = 1, Cancel = 2, WaitingApprove = 3
    }
    public record CreateFanMemberCommand
    {
        public string Email { get; set; }
        public string Document { get; set; }
    }
    public interface IFanMemberRepository
    {
        Task AddFanAsync(FanMember fanMember);

        Task<FanMember?> GetFanByDocument(string email, string document);
        //Task<long> DeleteAll();
    }
    internal class FanMemberHandler
    {
        private readonly IFanMemberRepository _repository;

        public FanMemberHandler(IFanMemberRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(CreateFanMemberCommand command)
        {
            try
            {
                var purchasedTicket = await _repository.GetFanByDocument(command.Email, command.Document);
                if (purchasedTicket != null)
                    return Result.Fail($"Já existe uma conta com o documento: {command.Document}.");

                var purchasableticket = command.MapTo<FanMember>();
                purchasableticket.Open();
                await _repository.AddFanAsync(purchasableticket);

                //var reservedticket = purchasableticket.MapTo<TicketReservedEvent>();
                //await _eventMessaging.SendTicketReservedAsync(reservedticket);

                return Result.Ok();
            }
            catch (Exception)
            {
                return Result.Fail("Erro ao tentar comprar um ingresso.");
            }
        }
    }
}
