using CoExittor.Common.DTO.Event;
using CoExittor.Common.Models;

namespace CoExittor.Api.Application.Services.Interfaces
{
    public interface IEventService
    {
        Task<List<Event>> GetAllEventsAsync(CancellationToken token);
        Task<Event> GetEventByCode(Guid eventCode, CancellationToken token);
        Task<Guid> CreateEvent(CreateEventDTO createEventDTO, CancellationToken token);
        Task ParticipateInEvent(Guid eventCode, ParticipateEventDTO participateEventDTO, CancellationToken token);
        Task<ResultDTO> GetEventResult(Guid eventCode, CancellationToken token);
        /// <summary>
        /// Завершить голосование по событию и зафиксировать выбранные даты
        /// </summary>
        Task AcceptEvent(Guid eventCode, CancellationToken token);
    }
}
