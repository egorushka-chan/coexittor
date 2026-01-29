using CoExittor.Api.Domain.Models.Interfaces;

namespace CoExittor.Api.Domain.Models
{
    public class Participant : IEntity
    {
        public long ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public long EventID { get; set; }
        public long? LinkedUserID { get; set; }

        public Event? Event { get; set; }
        public User? LinkedUser { get; set; }
        public bool IsAnonymous => LinkedUserID is null;
    }
}
