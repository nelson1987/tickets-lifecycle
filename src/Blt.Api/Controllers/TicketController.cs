using Blt.Core.Features.FanMember;
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
    private readonly IMatchRepository _matchRepository;
    private readonly IValidator<BuyTicketCommand> _validator;
    private readonly IBuyTicketHandler _handler;
    public TicketController(ITicketRepository repository,
        IValidator<BuyTicketCommand> validator,
        IBuyTicketHandler handler,
        IMatchRepository matchRepository)
    {
        _repository = repository;
        _validator = validator;
        _handler = handler;
        _matchRepository = matchRepository;
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

        var idGame = Guid.NewGuid();
        await _matchRepository.AddMatchAsync(new Game()
        {
            Id = idGame,
            Description = "Partida Futebol",
            Status = GameStatus.Available,
            Capacity = 5,
            AvailableTickets = 5
        });

        


        var result = await _handler.Handle(command, idGame);
        return result.IsFailed
            ? BadRequest(result.Errors)
            : StatusCode(201, "Ingresso reservado com sucesso");
    }
}
//[ApiController]
//[Route("[controller]")]
//public class MatchController : ControllerBase
//{
//    private readonly ITicketRepository _repository;
//    public MatchController(ITicketRepository repository)
//    {
//        _repository = repository;
//    }

//    [HttpGet("{data}", Name = "GetMatch")]
//    public async Task<ActionResult<GetTicketResponse>> GetByData(DateTime data)
//    {
//        var purchasedTicket = await _repository.GetEventByDocument(data);
//        if (purchasedTicket == null)
//            return NotFound();

//        var response = purchasedTicket!.MapTo<GetTicketResponse>();

//        return StatusCode(200, response);
//    }

//    [HttpPost(Name = "IncludeMatch")]
//    public async Task<ActionResult> Buy(BuyTicketCommand command)
//    {
//    }
//}