using Microsoft.Extensions.DependencyInjection;
using PoohAPI.Infrastructure.Common;
using PoohAPI.Infrastructure.UserDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Users.Services;

namespace PoohAPI.Logic.Users.Init
{
    public static class DIComponentsConfig
    {
        public static void RegisterComponents(IServiceCollection services)
        {
            services.AddScoped<IUserReadService, UserReadService>();
            services.AddScoped<IUserCommandService, UserCommandService>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
