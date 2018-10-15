using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Infrastructure.MapAPI.APIClients;
using PoohAPI.Logic.MapAPI.Services;

namespace PoohAPI.Logic.MapAPI.Init
{
    public static class DIComponentsConfig
    {
        public static void RegisterComponents(IServiceCollection services)
        {
            services.AddScoped<IMapAPIClient, AzureMapsAPIClient>();
            services.AddScoped<IMapAPIReadService, MapAPIReadService>();
        }
    }
}
