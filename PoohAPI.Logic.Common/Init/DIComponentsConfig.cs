using Microsoft.Extensions.DependencyInjection;
using PoohAPI.Infrastructure.Common;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Common.Classes;

namespace PoohAPI.Logic.Common.Init
{
    public static class DIComponentsConfig
    {
        public static void RegisterComponents(IServiceCollection services)
        {
            services.AddSingleton<IMySQLClient, MySQLClient>();
            services.AddScoped<IQueryBuilder, QueryBuilder>();
            services.AddScoped<IMailClient, MailClient>();
        }
    }
}
