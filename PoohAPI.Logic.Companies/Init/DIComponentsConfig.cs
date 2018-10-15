using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoohAPI.Infrastructure.CompanyDB.Respositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Companies.Services;

namespace PoohAPI.Logic.Companies.Init
{
    public static class DIComponentsConfig
    {
        public static void RegisterComponents(IServiceCollection services)
        {
            services.AddScoped<ICompanyReadService, CompanyReadService>();
            services.AddScoped<ICompanyCommandService, CompanyCommandService>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
        }
    }
}
