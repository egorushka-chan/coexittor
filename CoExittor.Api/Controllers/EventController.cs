using CoExittor.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoExittor.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        [HttpGet("all")]
        public IActionResult GetAllEvents()
        {
            throw new NotImplementedException();
        }

        [HttpGet("by-code/{eventCode}")]
        public IActionResult GetEventByCode([FromRoute] string eventCode)
        {
            throw new NotImplementedException();
        }

        [HttpPost("create")]
        public IActionResult CreateEvent()
        {
            throw new NotImplementedException();
        }

        [HttpPost("participate/{eventCode}")]
        public IActionResult ParticipateInEvent(
            [FromRoute] string eventCode,
            [FromBody] Participation participation)
        {
            throw new NotImplementedException();
        }

        [HttpGet("calculate/{eventCode}")]
        public IActionResult CalculateVote([FromRoute] string eventCode)
        {
            throw new NotImplementedException();
        }
    }
}
