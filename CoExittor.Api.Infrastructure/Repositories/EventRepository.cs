using CoExittor.Api.Domain.Repositories;
using CoExittor.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace CoExittor.Api.Infrastructure.Repositories
{
    internal class EventRepository(MainDbContext context) : BaseRepository<Event>(context), IEventRepository
    {
        public async Task<bool> AcceptEventAsync(Guid eventCode, CancellationToken token)
        {
            int rows = await _context.Events
                .Where(e => e.Code == eventCode)
                .ExecuteUpdateAsync(ev => ev.SetProperty(e => e.IsAccepted, true), token);
            return rows != 0;
        }

        public async Task<Event?> GetEventByCodeAsync(Guid eventCode, CancellationToken token)
        {
            return await _context.Events
                .Where(ev => ev.Code == eventCode)
                .Include(ev => ev.Participants)
                .ThenInclude(part => part.Votings)
                .AsNoTracking()
                .FirstOrDefaultAsync(token);
        }
    }
}
