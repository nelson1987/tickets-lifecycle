using Blt.Core.Features.Tickets;
using Blt.Core.Features.Tickets.BuyTickets;
using Blt.Core.Features.Tickets.GetTicket;
using Blt.Core.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Blt.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _repository;
        private readonly IEventMessaging _eventMessaging;

        public TicketController(ITicketRepository repository, IEventMessaging eventMessaging)
        {
            _repository = repository;
            _eventMessaging = eventMessaging;
        }

        [HttpGet(Name = "GetTicket")]
        [Route("{evento}/{documento}")]
        public async Task<ActionResult<GetTicketResponse>> GetById(string evento, string documento)
        {
            var purchasedTicket = await _repository.GetEventByDocument(evento, documento);
            if (purchasedTicket == null)
                return NotFound();

            var response = purchasedTicket!.MapTo<GetTicketResponse>();

            return StatusCode(200, response);
        }

        [HttpPost(Name = "BuyTicket")]
        public async Task<ActionResult> Buy(BuyTicketCommand command)
        {
            try
            {
                var purchasedTicket = await _repository.GetEventByDocument(command.Event, command.Document);
                if (purchasedTicket != null)
                    return BadRequest($"Já existe ticket para {command.Event} comprado com este documento.");

                var purchasableticket = command.MapTo<Ticket>();
                await _repository.AddTicketAsync(purchasableticket);

                var reservedticket = purchasableticket.MapTo<TicketReservedEvent>();
                await _eventMessaging.SendTicketReservedAsync(reservedticket);

                return StatusCode(201, "Ingresso reservado com sucesso");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao tentar comprar um ingresso.");
            }
        }
    }
}