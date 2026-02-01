using CoExittor.Api.Application.Services.Interfaces;
using CoExittor.Common.DTO.Event;
using CoExittor.Common.DTO.Message;
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
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DefaultErrorMessage))]
        public async Task<IActionResult> GetEventByCode([FromRoute] Guid eventCode, CancellationToken token)
        {
            Event? eventByCode = await _eventService.GetEventByCode(eventCode, token);
            return Ok(eventByCode);
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(DefaultErrorMessage))]
        public async Task<IActionResult> CreateEvent(CreateEventDTO createEventDTO, CancellationToken token)
        {
            Guid createdEventCode = await _eventService.CreateEvent(createEventDTO, token);
            return CreatedAtAction(nameof(GetEventByCode), new { eventCode = createdEventCode }, null);
        }

        [HttpPost("participate/{eventCode}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DefaultErrorMessage))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(DefaultErrorMessage))]
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
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DefaultErrorMessage))]
        public async Task<IActionResult> CalculateVote([FromRoute] Guid eventCode, CancellationToken token)
        {
            ResultDTO result = await _eventService.GetEventResult(eventCode, token);
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DefaultErrorMessage))]
        [HttpPost("accept/{eventCode}")]
        public async Task<IActionResult> AcceptEvent([FromRoute] Guid eventCode, CancellationToken token)
        {
            await _eventService.AcceptEvent(eventCode, token);
            return NoContent();
        }
    }
}
