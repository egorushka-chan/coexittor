using System.Runtime.CompilerServices;
using CoExittor.Api.Application.Services.Interfaces;
using CoExittor.Api.Domain.Repositories;
using CoExittor.Common.DTO.Event;
using CoExittor.Common.DTO.Voting;
using CoExittor.Common.Models;

[assembly: InternalsVisibleTo("CoExittor.Api.Tests")]

namespace CoExittor.Api.Application.Services
{

    internal class EventService : IEventService
    {
        private readonly IEventRepository _repository;
        private readonly IBaseRepository<Participation> _participationRepository;

        public EventService(IEventRepository repository, IBaseRepository<Participation> participationRepository)
        {
            _repository = repository;
            _participationRepository = participationRepository;
        }

        public async Task<ResultDTO> GetEventResult(Guid eventCode, CancellationToken token)
        {
            Event? @event = await _repository.GetEventByCodeAsync(eventCode, token);
            if (@event is null)
            {
                // Логика выброса ошибки
                return new ResultDTO(); // заглушка
            }

            List<Voting> agreedVotings = CalculateAgreedDate(@event);

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
                IsAgreedWithDefault = true
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
                // Ошибка: нет такого события
            }
        }

        public async Task<List<Event>> GetAllEventsAsync(CancellationToken token)
        {
            List<Event> allEvents = await _repository.GetAllAsync(token);

            return allEvents;
        }

        public async Task<Event?> GetEventByCode(Guid eventCode, CancellationToken token)
        {
            Event? eventByCode = await _repository.GetEventByCodeAsync(eventCode, token);

            return eventByCode;
        }

        public async Task ParticipateInEvent(Guid eventCode, ParticipateEventDTO participateEventDTO, CancellationToken token)
        {
            Event? eventByCode = await _repository.GetEventByCodeAsync(eventCode, token);

            if(eventByCode is null)
            {
                // Ошибка: события не существует
                return; // заглушка
            }
            if (eventByCode.IsAccepted)
            {
                // Ошибка: событие уже принято, нельзя участвовать
                return; // заглушка
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


        // Внутренние методы

        internal static List<Voting> CalculateAgreedDate(Event @event)
        {
            // Берем в расчёт всех, кроме и так согласных, и хоста
            var participations = @event.Participants
                .Where(p => p.IsAgreedWithDefault is false && p.IsHost is false);

            // Примем голосования хоста за базовые
            List<Voting> agreedVotings = @event.Participants.First(p => p.IsHost).Votings;
            foreach (var participation in participations)
            {
                List<Voting> participantVotings = participation.Votings;
                // Проверяем, что у каждого участника есть хотя бы одно пересечение с каждым из базовых голосований
                foreach (var participantVoting in participantVotings)
                {
                    bool hasOverlap = agreedVotings.Any(pv => IsVotingOverlap(participantVoting, pv));
                    if (!hasOverlap)
                    {
                        // Если у кого-то нет пересечений, то и нет согласованных дат
                        return [];
                    }

                    // Ищем пересечения диапазонов, и на их основе обновляем базовые голосования
                    List<Voting> newBaseVotings = [];
                    foreach (var baseVoting in agreedVotings)
                    {
                        if (IsVotingOverlap(baseVoting, participantVoting))
                        {
                            // Вычисляем пересечение
                            DateTime newStartDate = baseVoting.StartDate > participantVoting.StartDate
                                ? baseVoting.StartDate
                                : participantVoting.StartDate;

                            DateTime newEndDate = baseVoting.EndDate < participantVoting.EndDate
                                ? baseVoting.EndDate
                                : participantVoting.EndDate;

                            newBaseVotings.Add(new Voting
                            {
                                StartDate = newStartDate,
                                EndDate = newEndDate
                            });
                        }
                    }
                    agreedVotings = newBaseVotings;
                }
            }

            return agreedVotings;
        }

        private static bool IsVotingOverlap(Voting voting1, Voting voting2)
        {
            return voting1.StartDate < voting2.EndDate && voting2.StartDate < voting1.EndDate;
        }
    }
}
