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
        public async Task<IActionResult> GetEventByCode([FromRoute] string eventCode, CancellationToken token)
        {
            Event? eventByCode = await _eventService.GetEventByCode(eventCode, token);
            return Ok(eventByCode);
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateEvent(CreateEventDTO createEventDTO, CancellationToken token)
        {
            string createdEventCode = await _eventService.CreateEvent(createEventDTO, token);
            return CreatedAtAction(nameof(GetEventByCode), new { eventCode = createEventDTO });
        }

        [HttpPost("participate/{eventCode}")]
        public async Task<IActionResult> ParticipateInEvent(
            [FromRoute] string eventCode,
            [FromBody] ParticipateEventDTO participateEventDTO,
            CancellationToken token)
        {
            await _eventService.ParticipateInEvent(eventCode, participateEventDTO, token);
            return NoContent();
        }

        [HttpGet("calculate/{eventCode}")]
        public async Task<IActionResult> CalculateVote([FromRoute] string eventCode, CancellationToken token)
        {
            ResultDTO result = await _eventService.CalculateResult(eventCode, token);
            return Ok(result);
        }
    }
}
