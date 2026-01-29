using CoExittor.Api.Domain.Models.Interfaces;

namespace CoExittor.Api.Domain.Models
{
    public class Event : IEntity
    {
        public long ID { get; set; }
        public string Code { get; set; } = string.Empty;
        public bool HardTimed {  get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Навигационные свойства EF Core
        public List<Participation> Participants { get; set; } = [];
    }
}
