using CoExittor.Common.DTO.Voting;

namespace CoExittor.Api.Application.Services.Interfaces
{
    public interface IVotingService
    {
        Task UpdateVotingAsync(UpdateVotingDTO votingDTO, CancellationToken token);
    }
}
