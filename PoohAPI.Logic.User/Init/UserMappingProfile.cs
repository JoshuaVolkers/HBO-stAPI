using System.Data;
using AutoMapper;
using PoohAPI.Infrastructure.UserDB.Models;
using PoohAPI.Logic.Common;
using PoohAPI.Logic.Common.Models;

namespace PoohAPI.Logic.Users.Init
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<WPUser, User>()
                .ForMember(d => d.EmailAddress, o => o.MapFrom(s => s.user_email))
                .ForMember(d => d.NiceName, o => o.MapFrom(s => s.user_name))
                .ForMember(d => d.RegistrationDate, o => o.Ignore())
                .ForMember(d => d.Roles, o => o.MapFrom(s => s.user_role.ToString()))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.user_id))
                .ReverseMap();

            CreateMap<IDataReader, WPUser>().ConvertUsing<DataReaderTypeConverter<WPUser>>();
        }
    }
}
