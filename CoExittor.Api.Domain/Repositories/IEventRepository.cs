using CoExittor.Common.Models;

namespace CoExittor.Api.Domain.Repositories
{
    public interface IEventRepository
    {
        Task<Event?> GetEventByCodeAsync(string code, CancellationToken token);
    }
}
