using DeviceManagementSystem.Application.Abstractions;
using DeviceManagementSystem.Infrastructure.Data;
using DeviceManagementSystem.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceManagementSystem.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
                       var connectionString = configuration.GetConnectionString("ConnectionString") 
                ?? configuration["Database:ConnectionString"];
            
            services.AddSingleton<IDatabaseProvider>(
                new DatabaseProvider(connectionString)
            );

            services.AddScoped<IUserRepository, UserRepository>();
            
        }

    }
}
