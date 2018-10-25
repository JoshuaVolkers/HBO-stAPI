using System;
using System.Data;
using AutoMapper;
using PoohAPI.Infrastructure.OptionDB.Models;
using PoohAPI.Logic.Common;
using PoohAPI.Logic.Common.Models.OptionModels;

namespace PoohAPI.Logic.Options.Init
{
    public class OptionMappingProfile : Profile
    {
        public OptionMappingProfile()
        {
            CreateMap<DBMajor, Major>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.opl_id))
                .ForMember(d => d.CrohoNumber, o => o.MapFrom(s => s.opl_croho_nr))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.opl_naam))
                .ForMember(d => d.EducationLevel, o => o.MapFrom(s => s.opl_niveau))
                .ForMember(d => d.Active, o => o.MapFrom(s => s.opl_actief));

            CreateMap<DBEducationLevel, EducationLevel>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.opn_id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.opn_naam));

            CreateMap<DBAllowedEmailAddress, AllowedEmailAddress>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.se_id))
                .ForMember(d => d.EmailAddress, o => o.MapFrom(s => s.se_domein))
                .ForMember(d => d.EducationalInstitutionId, o => o.MapFrom(s => s.se_onderwijsinstelling_id));

            CreateMap<DBLanguage, Language>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.talen_id))
                .ForMember(d => d.LanguageName, o => o.MapFrom(s => s.talen_naam))
                .ForMember(d => d.LanguageIso, o => o.MapFrom(s => s.talen_iso));

            CreateMap<IDataReader, DBMajor>().ConvertUsing<DataReaderTypeConverter<DBMajor>>();
            CreateMap<IDataReader, DBEducationLevel>().ConvertUsing<DataReaderTypeConverter<DBEducationLevel>>();
            CreateMap<IDataReader, DBAllowedEmailAddress>().ConvertUsing<DataReaderTypeConverter<DBAllowedEmailAddress>>();
        }
    }
}
