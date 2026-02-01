using CoExittor.Common.DTO.Event;
using CoExittor.Common.Models;

namespace CoExittor.Front.Services.Interfaces
{
    public interface IBackendClient
    {
        Task<Guid> CreateEventAsync(CreateEventDTO dto, CancellationToken token);
        Task<Event> GetEventByCodeAsync(Guid eventCode, CancellationToken token);
        Task ParticipateAsync(Guid eventCode, ParticipateEventDTO dto, CancellationToken token);
        Task<ResultDTO> CalculateAsync(Guid eventCode, CancellationToken token);
        Task AcceptAsync(Guid eventCode, CancellationToken token);
        Task<List<Event>> GetAllEventsAsync(CancellationToken token);
    }
}
