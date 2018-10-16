using AutoMapper;
using PoohAPI.Infrastructure.VacancyDB.Models;
using PoohAPI.Logic.Common;
using PoohAPI.Logic.Common.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Vacancies.Init
{
    public class VacancyMappingProfile : Profile
    {
        public VacancyMappingProfile()
        {
            CreateMap<DBVacancy, Vacancy>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.vacature_id))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.vacature_text))
                .ForMember(d => d.ClosingDate, o => o.MapFrom(s => s.vacature_datum_verlopen))
                .ForMember(d => d.CreationDate, o => o.MapFrom(s => s.vacature_datum_plaatsing))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.vacature_titel))
                .ForMember(d => d.CompanyId, o => o.MapFrom(s => s.vacature_bedrijf_id))
                .ForMember(d => d.Language, o => o.MapFrom(s => s.talen_naam))
                .ForMember(d => d.Latitude, o => o.MapFrom(s => (double)s.vacature_breedtegraad))
                .ForMember(d => d.Longitude, o => o.MapFrom(s => (double)s.vacature_lengtegraad))
                .ForMember(d => d.Link, o => o.MapFrom(s => s.vacature_link))
                .ForMember(d => d.EducationalAttainment, o => o.MapFrom(s => s.opn_naam))
                .ForMember(d => d.Education, o => o.MapFrom(s => s.opleidingen))
                .ForMember(d => d.City, o => o.MapFrom(s => s.bedrijf_vestiging_plaats))
                .ReverseMap();


            CreateMap<IDataReader, DBVacancy>().ConvertUsing<DataReaderTypeConverter<DBVacancy>>();
        }
    }
}
