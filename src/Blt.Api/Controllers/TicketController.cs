using Blt.Core;
using Microsoft.AspNetCore.Mvc;

namespace Blt.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketController : ControllerBase
    {
        public TicketController()
        {
        }

        [HttpGet(Name = "GetTicket")]
        public async Task<IActionResult> GetById(GetTicketQuery query)
        {
            return Ok();
        }

        [HttpGet(Name = "PostTicket")]
        public async Task<IActionResult> Post(CreateTicketCommand command)
        {
            return CreatedAtAction();
        }
    }
}