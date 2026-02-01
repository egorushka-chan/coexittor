using CoExittor.Api.Domain.Repositories;
using CoExittor.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace CoExittor.Api.Infrastructure.Repositories
{
    internal class ParticipationRepository(MainDbContext context) : BaseRepository<Participation>(context), IParticipationRepository
    {
        public async Task<Participation?> GetParticipationByIdWithEventAsync(long id, CancellationToken token)
        {
            return await _context.Participations
                .Where(p => p.ID == id)
                .Include(p => p.Event)
                .AsNoTracking()
                .FirstOrDefaultAsync(token);
        }
    }
}
