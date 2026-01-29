using CoExittor.Api.Domain.Models;
using CoExittor.Api.Domain.Repositories;
using CoExittor.Api.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoExittor.Api.Infrastructure
{
    public static class InfrastructureInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MainDbContext>(contextOptions =>
            {
                contextOptions.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(2);
                });
            });

            services.RegisterRepositories();

            return services;
        }

        private static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
        }
    }
}
