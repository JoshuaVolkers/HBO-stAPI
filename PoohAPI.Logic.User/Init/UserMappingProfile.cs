using AutoMapper;
using PoohAPI.Infrastructure.UserDB.Models;
using System.Data;

namespace PoohAPI.Logic.Users.Init
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<WPUser, Common.Models.User>()
                .ForMember(d => d.EmailAddress, o => o.MapFrom(s => s.user_email))
                .ForMember(d => d.NiceName, o => o.MapFrom(s => s.display_name))
                .ForMember(d => d.RegistrationDate, o => o.MapFrom(s => s.user_registered))
                .ReverseMap();

            CreateMap<IDataReader, WPUser>()
                .ForMember(d => d.ID, o => o.MapFrom(s => s["ID"]))
                .ForMember(d => d.user_login, o => o.MapFrom(s => s["user_login"]))
                .ForMember(d => d.user_nicename, o => o.MapFrom(s => s["user_nicename"]))
                .ForMember(d => d.user_email, o => o.MapFrom(s => s["user_email"]))
                .ForMember(d => d.user_registered, o => o.MapFrom(s => s["user_registered"]))
                .ForMember(d => d.user_status, o => o.MapFrom(s => s["user_status"]))
                .ForMember(d => d.display_name, o => o.MapFrom(s => s["display_name"]));
        }
    }
}
