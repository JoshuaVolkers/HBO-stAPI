using Microsoft.Extensions.DependencyInjection;
using PoohAPI.Infrastructure.Common;
using PoohAPI.Infrastructure.OptionDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Options.Services;

namespace PoohAPI.Logic.Options.Init
{
    public static class DIComponentsConfig
    {
        public static void RegisterComponents(IServiceCollection services)
        {
            services.AddScoped<IOptionReadService, OptionReadService>();
            services.AddScoped<IOptionRepository, OptionRepository>();
        }
    }
}
