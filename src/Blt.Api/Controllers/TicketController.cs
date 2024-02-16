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
        public async Task<IActionResult> Get()
        {
            return Ok();
        }
    }
}