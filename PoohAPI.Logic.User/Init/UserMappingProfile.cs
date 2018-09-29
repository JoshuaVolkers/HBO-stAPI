using AutoMapper;
using PoohAPI.Infrastructure.UserDB.Models;

namespace PoohAPI.Logic.Users.Init
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<WordPressPCL.Models.User, WPUser>()
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName))
                .ForMember(d => d.RegisteredDate, o => o.MapFrom(s => s.RegisteredDate))
                .ForMember(d => d.Meta, o => o.MapFrom(s => s.Meta));

            CreateMap<WPUser, Common.Models.User>()
                .ForMember(d => d.EmailAddress, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.NiceName, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.RegistrationDate, o => o.MapFrom(s => s.RegisteredDate));
        }
    }
}
