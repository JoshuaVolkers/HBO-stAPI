using Microsoft.Extensions.DependencyInjection;
using PoohAPI.Infrastructure.Common;
using PoohAPI.Infrastructure.ReviewDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Reviews.Services;

namespace PoohAPI.Logic.Reviews.Init
{
    public static class DIComponentsConfig
    {
        public static void RegisterComponents(IServiceCollection services)
        {
            services.AddScoped<IReviewReadService, ReviewReadService>();
            services.AddScoped<IReviewCommandService, ReviewCommandService>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddSingleton<IMySQLClient, MySQLClient>();
        }
    }
}