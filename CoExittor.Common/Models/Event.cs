using CoExittor.Common.Models.Interfaces;

namespace CoExittor.Common.Models
{
    /// <summary>
    /// Событие (поход куда-то)
    /// </summary>
    /// <remarks>
    /// Событие делятся на фиксированные и не фиксированные неявно.
    /// Фиксированные имеют только одну пару дат начала и конца, которые задал создатель.
    /// </remarks>
    public class Event : IEntity
    {
        public long ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        /// <summary>
        /// Уникальный код события, используется для ссылок
        /// </summary>
        public Guid Code { get; set; }
        /// <summary>
        /// Даты сошлись, и создатель завершил выбор
        /// </summary>
        public bool IsAccepted { get; set; }

        // Навигационные свойства EF Core
        public List<Participation> Participants { get; set; } = [];
    }
}
