using CoExittor.Api.Application.Services.Interfaces;
using CoExittor.Api.Domain.Exceptions;
using CoExittor.Api.Domain.Helpers;
using CoExittor.Api.Domain.Repositories;
using CoExittor.Common.DTO.Event;
using CoExittor.Common.DTO.Voting;
using CoExittor.Common.Models;

namespace CoExittor.Api.Application.Services
{

    internal class EventService : IEventService
    {
        private readonly IEventRepository _repository;
        private readonly IParticipationRepository _participationRepository;

        public EventService(IEventRepository repository, IParticipationRepository participationRepository)
        {
            _repository = repository;
            _participationRepository = participationRepository;
        }

        public async Task<ResultDTO> GetEventResult(Guid eventCode, CancellationToken token)
        {
            Event? @event = await _repository.GetEventByCodeAsync(eventCode, token)
                ?? throw new EntityNotFoundException("События с таким кодом не существует");

            List<Voting> agreedVotings = EventVoteHelpers.CalculateAgreedDate(@event);

            return new ResultDTO
            {
                AgreedDates = [.. agreedVotings.Select(voting => new VotingDTO
                {
                    StartDate = voting.StartDate,
                    EndDate = voting.EndDate
                })]
            };
        }

        public async Task<Guid> CreateEvent(CreateEventDTO createEventDTO, CancellationToken token)
        {
            EventVoteHelpers.CheckVoting(createEventDTO.Host.Votings);

            Guid guid = Guid.NewGuid();
            Event newEvent = new()
            {
                Code = guid,
                Name = createEventDTO.Name,
                Description = createEventDTO.Description,
                IsAccepted = false
            };

            Participation hostParticipation = new()
            {
                Event = newEvent,
                Name = createEventDTO.Host.Name,
                LinkedUserID = createEventDTO.Host.LinkedUserID,
                IsAgreedWithDefault = true,
                IsHost = true
            };
            hostParticipation.Votings = [.. createEventDTO.Host.Votings.Select(votingDTO => new Voting
            {
                Participation = hostParticipation,
                StartDate = votingDTO.StartDate,
                EndDate = votingDTO.EndDate,
            })];
            newEvent.Participants.Add(hostParticipation);
            await _repository.CreateAsync(newEvent, token);
            return newEvent.Code;
        }

        public async Task AcceptEvent(Guid eventCode, CancellationToken token)
        {
            bool isEventExists = await _repository.AcceptEventAsync(eventCode, token);
            if(isEventExists is false)
            {
                throw new EntityNotFoundException("События с таким кодом не существует");
            }
        }

        public async Task<List<Event>> GetAllEventsAsync(CancellationToken token)
        {
            List<Event> allEvents = await _repository.GetAllAsync(token);

            return allEvents;
        }

        public async Task<Event> GetEventByCode(Guid eventCode, CancellationToken token)
        {
            Event? eventByCode = await _repository.GetEventByCodeAsync(eventCode, token) ??
                throw new EntityNotFoundException("События с таким кодом не существует");

            return eventByCode;
        }

        public async Task ParticipateInEvent(Guid eventCode, ParticipateEventDTO participateEventDTO, CancellationToken token)
        {
            Event? eventByCode = await _repository.GetEventByCodeAsync(eventCode, token) 
                ?? throw new EntityNotFoundException("События с таким кодом не существует");
            if (eventByCode.IsAccepted)
            {
                throw new BadRequestException("Событие уже принято и не принимает новых участников");
            }

            Participation participation = new()
            {
                EventID = eventByCode.ID,
                Name = participateEventDTO.Name,
                LinkedUserID = participateEventDTO.LinkedUserID,
                IsAgreedWithDefault = participateEventDTO.IsAgreedWithDefault,
                IsHost = false
            };

            if(participateEventDTO.IsAgreedWithDefault is false)
            {
                if (participateEventDTO.Votings.Count == 0)
                    return; // Выброс ошибки

                foreach(VotingDTO votingDTO in participateEventDTO.Votings)
                {
                    Voting voting = new()
                    {
                        Participation = participation,
                        StartDate = votingDTO.StartDate,
                        EndDate = votingDTO.EndDate,
                    };
                    participation.Votings.Add(voting);
                }
            }

            await _participationRepository.CreateAsync(participation, token);
        }
    }
}
