using CoExittor.Api.Domain.Models.Interfaces;

namespace CoExittor.Api.Domain.Models
{
    public class User : IEntity
    {
        public long ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        // Навигационные свойства EF Core
        public List<Participation> Participations { get; set; } = [];
    }
}
