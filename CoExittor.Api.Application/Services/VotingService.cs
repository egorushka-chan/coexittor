using CoExittor.Api.Application.Services.Interfaces;
using CoExittor.Api.Domain.Exceptions;
using CoExittor.Api.Domain.Helpers;
using CoExittor.Api.Domain.Repositories;
using CoExittor.Common.DTO.Voting;
using CoExittor.Common.Models;

namespace CoExittor.Api.Application.Services
{
    internal class VotingService : IVotingService
    {
        private readonly IVotingRepository _repository;
        private readonly IParticipationRepository _participationRepository;

        public VotingService(IVotingRepository repository, IParticipationRepository participationRepository)
        {
            _repository = repository;
            _participationRepository = participationRepository;
        }

        public async Task UpdateVotingAsync(UpdateVotingDTO votingDTO, CancellationToken token)
        {
            Participation participation = await _participationRepository.GetParticipationByIdWithEventAsync(votingDTO.ParticipationID, token)
                ?? throw new EntityNotFoundException("Такого участника не существует");
            if (participation.Event?.IsAccepted == true)
            {
                throw new BadRequestException("Событие этого участника уже закончилось");
            }

            EventVoteHelpers.CheckVoting(votingDTO.Votings);

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
