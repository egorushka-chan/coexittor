using CoExittor.Api.Domain.Models.Interfaces;

namespace CoExittor.Api.Domain.Models
{
    public class EventParticipation : IEntity
    {
        public long ID { get; set; }
        public long EventID { get; set; }
        public long ParticipantID { get; set; }
        public bool IsAgreedWithDefault { get; set; }

        public Event? Event { get; set; }
        public Participant? Participant { get; set; }
        public List<EventVoting> EventVotings { get; set; } = [];
    }
}
