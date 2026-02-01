using CoExittor.Common.Models;

namespace CoExittor.Api.Domain.Repositories
{
    public interface IVotingRepository : IBaseRepository<Voting>
    {
        public Task ReplaceVotingToParticipantAsync(long participantId, List<Voting> votings, CancellationToken token);
    }
}
