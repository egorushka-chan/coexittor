using CoExittor.Api.Domain.Models.Interfaces;

namespace CoExittor.Api.Domain.Models
{
    public class EventVoting : IEntity
    {
        public long ID { get; set; }
        public long EventParticipationID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public EventParticipation? EventParticipation { get; set; }
        public Participant? Participant { get; set; }
    }
}
