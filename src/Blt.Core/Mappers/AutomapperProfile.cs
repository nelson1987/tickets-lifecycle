﻿using AutoMapper;
using Blt.Core.Features.Tickets;
using Blt.Core.Features.Tickets.BuyTickets;

namespace Blt.Core.Mappers
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<BuyTicketCommand, Ticket>()
                .ForMember(x => x.Id, y => y.Ignore());
            CreateMap<Ticket, TicketReservedEvent>();
        }
    }
}