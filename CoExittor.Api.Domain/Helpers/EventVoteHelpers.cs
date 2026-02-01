using CoExittor.Api.Domain.Exceptions;
using CoExittor.Common.DTO.Voting;
using CoExittor.Common.Models;

namespace CoExittor.Api.Domain.Helpers
{
    /// <summary>
    /// Вспомогательные методы для работы с голосованиями
    /// </summary>
    public static class EventVoteHelpers
    {
        public static List<Voting> CalculateAgreedDate(Event @event)
        {
            // Берем в расчёт всех, кроме и так согласных, и хоста
            var participations = @event.Participants
                .Where(p => p.IsAgreedWithDefault is false && p.IsHost is false)
                .ToList();

            // Примем голосования хоста за базовые
            List<Voting> agreedVotings = @event.Participants.First(p => p.IsHost).Votings;
            foreach (var participation in participations)
            {
                List<Voting> participantVotings = participation.Votings;
                // Проверяем, что у каждого участника есть хотя бы одно пересечение с каждым из базовых голосований
                List<Voting> newBaseVotings = [];
                foreach (var participantVoting in participantVotings)
                {
                    bool hasOverlap = agreedVotings.Any(pv => IsVotingOverlap(participantVoting, pv));
                    if (!hasOverlap)
                    {
                        // Если у кого-то нет пересечений, то и нет согласованных дат
                        continue;
                    }

                    // Ищем пересечения диапазонов, и на их основе обновляем базовые голосования
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
                }
                agreedVotings = newBaseVotings;
            }

            return agreedVotings;
        }

        public static bool IsVotingOverlap(Voting voting1, Voting voting2)
        {
            return voting1.StartDate < voting2.EndDate && voting2.StartDate < voting1.EndDate;
        }

        public static Voting? SearchForIncorrectVoting(IEnumerable<Voting> votings)
        {
            foreach (var voting in votings)
            {
                if (voting.EndDate < voting.StartDate)
                {
                    return voting;
                }
                if (voting.StartDate < DateTime.Now)
                {
                    return voting;
                }
            }
            return null;
        }

        public static void CheckVoting(IEnumerable<VotingDTO> votings)
        {
            List<VotingDTO> incorrects = [];
            foreach (var voting in votings)
            {
                if (voting.EndDate < voting.StartDate)
                {
                    incorrects.Add(voting);
                }
                else if (voting.StartDate < DateTime.Now)
                {
                    incorrects.Add(voting);
                }
            }

            if (incorrects.Count > 0)
            {
                List<string> errors = [];
                foreach (var incorrect in incorrects)
                {
                    string message = $"Некорректное голосование " +
                        $"с датами с '{incorrect.StartDate}' по '{incorrect.EndDate}'";
                    errors.Add(message);
                }

                throw new BadRequestException(new Dictionary<string, List<string>>
                {
                     { "Voting", errors}
                });
            }
        }
    }
}
