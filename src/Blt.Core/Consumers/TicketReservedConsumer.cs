using Blt.Core.Features.FanMember;
using Blt.Core.Features.Tickets;
using Blt.Core.Features.Tickets.BuyTickets;
using MassTransit;

namespace Blt.Core.Consumers;
public class TicketReservedConsumer : IConsumer<TicketReservedEvent>
{
    private readonly IFanMemberRepository _FanMemberRepository;
    private readonly ITicketRepository _ticketRepository;

    public TicketReservedConsumer(IFanMemberRepository fanMemberRepository, 
        ITicketRepository ticketRepository)
    {
        _FanMemberRepository = fanMemberRepository;
        _ticketRepository = ticketRepository;
    }

    public async Task Consume(ConsumeContext<TicketReservedEvent> context)
    {
        //var message = context.Message;
        //if (message == null) 
        //{
        //    var member = await _FanMemberRepository.GetFanByDocument("", message!.Document);
        //    //if(member == null)
        //    //if (member.Status != FanMemberStatus.Available) { }
        //    _ticketRepository.GetEventByDocument
        //}

        //Só pode comprar ingresso Member que estiver com status aberto e com saldo disponível.
        //Se estiver aberto mas sem saldo, devemos enviar para a fila de notificação.
        //Caso contrário será rejeitada a solicitação
        throw new NotImplementedException();
        //return Task.CompletedTask;
    }
}
