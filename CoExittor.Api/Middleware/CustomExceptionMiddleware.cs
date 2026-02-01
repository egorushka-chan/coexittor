using System.Net;
using System.Text.Json;
using CoExittor.Api.Domain.Exceptions.Interfaces;
using CoExittor.Common.DTO.Message;

namespace CoExittor.Api.Middleware
{
    /// <summary>
    /// Middleware, который обрабатывает ошибки в HTTP ответы 
    /// </summary>
    /// <remarks>
    /// Люблю я эту штуку
    /// </remarks>
    public class CustomExceptionMiddleware
    {
        private const string UNEXPECTED_ERROR_TEXT = "Непредвиденная ошибка";

        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environment, ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _environment = environment;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error {exceptionName}:", exception.GetType().Name);
                DefaultErrorMessage response = exception switch
                {
                    IMessageException ex => ex.ToErrorMessage(),
                    _ => new DefaultErrorMessage()
                    {
                        Title = UNEXPECTED_ERROR_TEXT,
                        Status = (int)HttpStatusCode.InternalServerError,
                        TimeStamp = DateTime.Now,
                        Details = exception.Message
                    }
                };

                context.Response.StatusCode = response.Status;
                context.Response.ContentType = "application/problem+json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }

    public static class CustomExceptionMiddlewareExtensions
    {
        extension(IApplicationBuilder builder)
        {
            public IApplicationBuilder UseExceptionMiddleware()
            {
                return builder.UseMiddleware<CustomExceptionMiddleware>();
            }
        }
    }
}
