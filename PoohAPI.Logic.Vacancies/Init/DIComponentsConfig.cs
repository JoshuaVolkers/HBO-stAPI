using Microsoft.Extensions.DependencyInjection;
using PoohAPI.Infrastructure.VacancyDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Vacancies.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Vacancies.Init
{
    public class DIComponentsConfig
    {
        public static void RegisterComponents(IServiceCollection services)
        {
            services.AddScoped<IVacancyReadService, VacancyReadService>();
            services.AddScoped<IVacancyCommandService, VacancyCommandService>();
            services.AddScoped<IVacancyRepository, VacancyRepository>();
        }
    }
}
