using CoExittor.Api.Application.Services.Interfaces;
using CoExittor.Api.Domain.Repositories;
using CoExittor.Common.DTO.Voting;
using CoExittor.Common.Models;

namespace CoExittor.Api.Application.Services
{
    internal class VotingService : IVotingService
    {
        private readonly IVotingRepository _repository;
        public async Task UpdateVotingAsync(UpdateVotingDTO votingDTO, CancellationToken token)
        {
            List<Voting> newVoting = [.. votingDTO.Votings.Select(voting => new Voting
            {
                ParticipationID = votingDTO.ParticipationID,
                StartDate = voting.StartDate,
                EndDate = voting.EndDate
            })];

            await _repository.ReplaceVotingToParticipantAsync(votingDTO.ParticipationID, newVoting, token);
        }
    }
}
