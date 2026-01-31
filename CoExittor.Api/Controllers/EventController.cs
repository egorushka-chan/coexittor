using CoExittor.Api.Application.Services.Interfaces;
using CoExittor.Common.DTO.Event;
using CoExittor.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoExittor.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Event>))]
        public async Task<IActionResult> GetAllEvents(CancellationToken token)
        {
            List<Event>? events = await _eventService.GetAllEventsAsync(token);
            return Ok(events);
        }

        [HttpGet("by-code/{eventCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Event))]
        public async Task<IActionResult> GetEventByCode([FromRoute] Guid eventCode, CancellationToken token)
        {
            Event? eventByCode = await _eventService.GetEventByCode(eventCode, token);
            return Ok(eventByCode);
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateEvent(CreateEventDTO createEventDTO, CancellationToken token)
        {
            Guid createdEventCode = await _eventService.CreateEvent(createEventDTO, token);
            return CreatedAtAction(nameof(GetEventByCode), new { eventCode = createEventDTO.ToString() });
        }

        [HttpPost("participate/{eventCode}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ParticipateInEvent(
            [FromRoute] Guid eventCode,
            [FromBody] ParticipateEventDTO participateEventDTO,
            CancellationToken token)
        {
            await _eventService.ParticipateInEvent(eventCode, participateEventDTO, token);
            return NoContent();
        }

        [HttpGet("calculate/{eventCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultDTO))]
        public async Task<IActionResult> CalculateVote([FromRoute] Guid eventCode, CancellationToken token)
        {
            ResultDTO result = await _eventService.GetEventResult(eventCode, token);
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPost("accept/{eventCode}")]
        public async Task<IActionResult> AcceptEvent([FromRoute] Guid eventCode, CancellationToken token)
        {
            await _eventService.AcceptEvent(eventCode, token);
            return NoContent();
        }
    }
}
