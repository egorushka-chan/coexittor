using CoExittor.Api.Domain.Models.Interfaces;

namespace CoExittor.Api.Domain.Models
{
    public class Voting : IEntity
    {
        public long ID { get; set; }
        public long ParticipationID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Навигационные свойства EF Core
        public Participation? Participation { get; set; }
    }
}
