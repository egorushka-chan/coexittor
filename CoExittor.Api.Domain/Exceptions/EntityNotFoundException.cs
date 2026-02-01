using CoExittor.Api.Domain.Exceptions.Interfaces;
using CoExittor.Common.DTO.Message;

namespace CoExittor.Api.Domain.Exceptions
{
    /// <summary>
    /// Ошибка, выбрасывается в случае, если данные по переданному ключу не найдены
    /// </summary>
    public class EntityNotFoundException : Exception, IMessageException
    {
        public string Title { get; set; } = "По переданному ключу ничего не найдено";
        public int StatusCode { get; set; } = 404;

        public EntityNotFoundException()
        : base("Ошибка.")
        { }

        public EntityNotFoundException(string message)
            : base(message)
        { }

        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public DefaultErrorMessage ToErrorMessage()
        {
            DefaultErrorMessage message = new()
            {
                Title = this.Title,
                Status = this.StatusCode,
                Details = this.Message,
                TimeStamp = DateTime.Now
            };
            return message;
        }
    }
}
