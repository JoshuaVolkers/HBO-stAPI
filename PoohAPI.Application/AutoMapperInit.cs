using AutoMapper;
using AutoMapper.Data;
using PoohAPI.Logic.Users.Init;
using PoohAPI.Logic.Companies.Init;
using PoohAPI.Logic.Vacancies.Init;
using PoohAPI.Logic.Reviews.Init;

namespace PoohAPI.Application
{
    public static class AutoMapperInit
    {
        public static MapperConfiguration InitMappings()
        {
            return new MapperConfiguration(mc =>
            {
                mc.AddDataReaderMapping();
                mc.AddProfile(new UserMappingProfile());
                mc.AddProfile(new CompanyMappingProfile());
                mc.AddProfile(new VacancyMappingProfile());
                mc.AddProfile(new ReviewMappingProfile());
            });
        }
    }
}
