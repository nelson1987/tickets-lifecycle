using AutoMapper;
using Blt.Core.Features.Tickets;
using Blt.Core.Features.Tickets.BuyTickets;
using Blt.Core.Features.Tickets.GetTicket;

namespace Blt.Core.Mappers;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<BuyTicketCommand, Ticket>()
            .ForMember(x => x.Id, y => y.Ignore())
            .ForMember(x => x.Status, y => y.Ignore());
        CreateMap<Ticket, TicketReservedEvent>()
            .ForMember(x => x.GameId, y => y.Ignore());
        CreateMap<Ticket, GetTicketResponse>();
    }
}