using Microsoft.Extensions.DependencyInjection;
using PoohAPI.Infrastructure.Common;
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
        }
    }
}
