namespace CoExittor.Common.DTO.Message
{
    /// <summary>
    /// Базовое сообщение об ошибке
    /// </summary>
    /// <remarks>
    /// Структура похожа на <see cref="ProblemDetails"/>
    /// </remarks>
    public class DefaultErrorMessage
    {
        /// <summary>
        /// Общее описание ошибки
        /// </summary>
        public required string Title { get; set; }
        /// <summary>
        /// HTTP статус-код
        /// </summary>
        public required int Status { get; set; }
        /// <summary>
        /// Время генерации ошибки
        /// </summary>
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// Конкретное описание ошибки
        /// </summary>
        public string Details { get; set; } = string.Empty;
        /// <summary>
        /// Поле с ошибками валидации 
        /// </summary>
        public Dictionary<string, string[]>? Errors { get; set; }
    }
}
