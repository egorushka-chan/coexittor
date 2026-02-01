using CoExittor.Common.Models;

namespace CoExittor.Api.Domain.Repositories
{
    public interface IParticipationRepository : IBaseRepository<Participation>
    {
        Task<Participation?> GetParticipationByIdWithEventAsync(long id, CancellationToken token);
    }
}
