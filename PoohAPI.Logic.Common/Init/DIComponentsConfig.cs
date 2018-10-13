using Microsoft.Extensions.DependencyInjection;
using PoohAPI.Infrastructure.Common;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Init
{
    public static class DIComponentsConfig
    {
        public static void RegisterComponents(IServiceCollection services)
        {
            services.AddSingleton<IMySQLClient, MySQLClient>();
            services.AddScoped<IQueryBuilder, QueryBuilder>();
        }
    }
}
