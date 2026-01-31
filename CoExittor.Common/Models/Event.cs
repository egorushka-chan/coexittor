using CoExittor.Common.Models.Interfaces;

namespace CoExittor.Common.Models
{
    public class Event : IEntity
    {
        public long ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Desciprion { get; set; }
        /// <summary>
        /// Уникальный код события, используется для ссылок
        /// </summary>
        public string Code { get; set; } = string.Empty;
        /// <summary>
        /// Фиксирована ли дата события
        /// </summary>
        public bool IsHardTimed {  get; set; }
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Конец временного диапазона
        /// </summary>
        /// <remarks>
        /// Если null - то событие строго в StartDate
        /// </remarks>
        public DateTime? EndDate { get; set; }

        // Навигационные свойства EF Core
        public List<Participation> Participants { get; set; } = [];
    }
}
