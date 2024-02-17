using Blt.Core.Features.Tickets;
using Blt.Core.Features.Tickets.BuyTickets;
using Blt.Core.Features.Tickets.GetTicket;
using Blt.Core.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Blt.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketController : ControllerBase
{
    private readonly ITicketRepository _repository;
    private readonly IValidator<BuyTicketCommand> _validator;
    private readonly IBuyTicketHandler _handler;
    public TicketController(ITicketRepository repository,
        IValidator<BuyTicketCommand> validator,
        IBuyTicketHandler handler)
    {
        _repository = repository;
        _validator = validator;
        _handler = handler;
    }

    [HttpGet("{evento}/{documento}", Name = "GetTicket")]
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
        var validation = _validator.Validate(command);
        if (validation.IsInvalid())
            return UnprocessableEntity(validation.ToModelState());

        var result = await _handler.Handle(command);
        return result.IsFailed
            ? BadRequest(result.Errors)
            : StatusCode(201, "Ingresso reservado com sucesso");
    }
}