using CoExittor.Common.Models;

namespace CoExittor.Api.Domain.Repositories
{
    public interface IEventRepository : IBaseRepository<Event>
    {
        Task<Event?> GetEventByCodeAsync(Guid eventCode, CancellationToken token);
        Task<bool> AcceptEventAsync(Guid eventCode, CancellationToken token);
        Task<List<Event>> GetFullAllAsync(CancellationToken token);
    }
}
