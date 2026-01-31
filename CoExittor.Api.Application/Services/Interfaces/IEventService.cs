using CoExittor.Common.DTO.Event;
using CoExittor.Common.Models;

namespace CoExittor.Api.Application.Services.Interfaces
{
    public interface IEventService
    {
        Task<List<Event>> GetAllEventsAsync(CancellationToken token);
        Task<Event?> GetEventByCode(string code, CancellationToken token);
        Task<string> CreateEvent(CreateEventDTO createEventDTO, CancellationToken token);
        Task ParticipateInEvent(string eventCode, ParticipateEventDTO participateEventDTO, CancellationToken token);
        Task<ResultDTO> CalculateResult(string eventCode, CancellationToken token);
    }
}
