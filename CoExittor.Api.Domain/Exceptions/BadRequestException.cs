using CoExittor.Api.Domain.Exceptions.Interfaces;
using CoExittor.Common.DTO.Message;

namespace CoExittor.Api.Domain.Exceptions
{
    /// <summary>
    /// Ошибка, выбрасывается в случаях, когда получены некорректные данные
    /// </summary>
    public class BadRequestException : Exception, IMessageException
    {
        public string Title { get; set; } = "Произошла одна или более ошибок валидации.";
        public int StatusCode { get; set; } = 400;
        public Dictionary<string, List<string>> Errors { get; set; } = [];

        public BadRequestException()
        {

        }

        public BadRequestException(Dictionary<string, List<string>> errors) : base()
        {
            Errors = errors;
        }

        public BadRequestException(string message) : base(message)
        {

        }

        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public DefaultErrorMessage ToErrorMessage()
        {
            DefaultErrorMessage message = new()
            {
                Title = this.Title,
                Status = this.StatusCode,
                Errors = this.Errors.Select(x => new KeyValuePair<string, string[]>(x.Key, [.. x.Value])).ToDictionary(),
                Details = this.Message,
                TimeStamp = DateTime.Now
            };
            return message;
        }
    }
}
