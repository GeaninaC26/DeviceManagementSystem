using DeviceManagementSystem.Application.Contracts;
using DeviceManagementSystem.Application.Features.Devices;
using DeviceManagementSystem.Application.Features.UserDevices;
using DeviceManagementSystem.Application.Features.Users;
using DeviceManagementSystem.Application.Mappers;
using DeviceManagementSystem.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace DeviceManagementSystem.Application
{

    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
                services.AddScoped<DeviceService>();
                services.AddScoped<UserDeviceService>();
                services.AddScoped<UserService>();
                services.AddScoped<UserMapper>();
                services.AddScoped<DeviceMapper>();
                services.AddScoped<UserDeviceMapper>();
        }
    }

}