using CoExittor.Common.Models.Interfaces;

namespace CoExittor.Common.Models
{
    public class Participation : IEntity
    {
        public long ID { get; set; }
        public long EventID { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsHost { get; set; }
        public bool IsAgreedWithDefault { get; set; }
        public long? LinkedUserID { get; set; }

        public bool IsAnonymous => LinkedUserID is null;

        // Навигационные свойства EF Core
        public Event? Event { get; set; }
        public User? LinkedUser { get; set; }
        public List<Voting> Votings { get; set; } = [];

    }
}
