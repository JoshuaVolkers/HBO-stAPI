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
                .ForMember(d => d.Active, o => o.MapFrom(s => s.opl_actief))
                .ReverseMap();            

            CreateMap<IDataReader, DBMajor>().ConvertUsing<DataReaderTypeConverter<DBMajor>>();
        }
    }
}
