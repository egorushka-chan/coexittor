using CoExittor.Api.Domain.Models.Interfaces;

namespace CoExittor.Api.Domain.Models
{
    public class Event : IEntity
    {
        public long ID { get; set; }
        public string Code { get; set; } = string.Empty;
        public long HostID { get; set; }
        public bool HardTimed {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Participant? Host { get; set; }
        public List<EventVoting> EventVotings { get; set; } = [];
    }
}
