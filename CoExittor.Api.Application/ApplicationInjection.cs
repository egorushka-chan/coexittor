using CoExittor.Api.Application.Services;
using CoExittor.Api.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CoExittor.Api.Application
{
    public static class ApplicationInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IVotingService, VotingService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
