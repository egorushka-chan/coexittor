using Microsoft.Extensions.DependencyInjection;

namespace CoExittor.Api.Application
{
    public static class ApplicationInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            return services;
        }
    }
}
