using CoExittor.Api.Domain.Repositories;
using CoExittor.Common.Models;

namespace CoExittor.Api.Infrastructure.Repositories
{
    internal class EventRepository(MainDbContext context) : BaseRepository<Event>(context), IEventRepository
    {
        public Task<Event?> GetEventByCodeAsync(string code, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
