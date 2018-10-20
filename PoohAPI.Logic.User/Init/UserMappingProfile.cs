using System.Data;
using AutoMapper;
using PoohAPI.Infrastructure.UserDB.Models;
using PoohAPI.Logic.Common;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.BaseModels;

namespace PoohAPI.Logic.Users.Init
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<DBUser, BaseUser>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.user_id))
                .ForMember(d => d.NiceName, o => o.MapFrom(s => s.user_name))
                .ForMember(d => d.EmailAddress, o => o.MapFrom(s => s.user_email))
                .ForMember(d => d.Role, o => o.MapFrom(s => s.user_role))
                .ReverseMap();

            CreateMap<DBUser, User>()
                .IncludeBase<DBUser, BaseUser>()
                .ForMember(d => d.EducationalAttainmentId, o => o.MapFrom(s => s.user_op_niveau))
                .ForMember(d => d.EducationalAttainment, o => o.MapFrom(s => s.opn_naam))
                .ForMember(d => d.EducationId, o => o.MapFrom(s => s.user_opleiding_id))
                .ForMember(d => d.Education, o => o.MapFrom(s => s.opl_naam))
                .ForMember(d => d.Location, o => o.MapFrom(s => s))
                .ForMember(d => d.PreferredLanguageId, o => o.MapFrom(s => s.user_taal))
                .ForMember(d => d.PreferredLanguage, o => o.MapFrom(s => s.talen_naam))
                .ReverseMap();

            CreateMap<DBUser, BaseLocation>()
                .ForMember(d => d.CountryId, o => o.MapFrom(s => s.user_land))
                .ForMember(d => d.CountryName, o => o.MapFrom(s => s.land_naam))
                .ForMember(d => d.City, o => o.MapFrom(s => s.user_woonplaats))
                .ForMember(d => d.Latitude, o => o.MapFrom(s => s.user_breedtegraad))
                .ForMember(d => d.Longitude, o => o.MapFrom(s => s.user_lengtegraad))
                .ReverseMap();

            CreateMap<DBUserEmailVerification, UserEmailVerification>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.ver_id))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.ver_user_id))
                .ForMember(d => d.Token, o => o.MapFrom(s => s.ver_token))
                .ForMember(d => d.ExpirationDate, o => o.MapFrom(s => s.ver_expiration))
                .ReverseMap();


            CreateMap<IDataReader, DBUser>().ConvertUsing<DataReaderTypeConverter<DBUser>>();

            CreateMap<IDataReader, DBUserEmailVerification>().ConvertUsing<DataReaderTypeConverter<DBUserEmailVerification>>();
        }
    }
}
