using CoExittor.Api.Domain.Repositories;
using CoExittor.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace CoExittor.Api.Infrastructure.Repositories
{
    internal class VotingRepository(MainDbContext context) : BaseRepository<Voting>(context), IVotingRepository
    {
        public async Task ReplaceVotingToParticipantAsync(long participantId, List<Voting> votings, CancellationToken token)
        {
            await _context.Votings
                .Where(v => v.ParticipationID == participantId)
                .ExecuteDeleteAsync(cancellationToken: token);
            _context.Votings.AddRange(votings);
            await _context.SaveChangesAsync(CancellationToken.None);
        }
    }
}
