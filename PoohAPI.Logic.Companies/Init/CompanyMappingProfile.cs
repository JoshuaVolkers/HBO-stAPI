using AutoMapper;
using PoohAPI.Infrastructure.CompanyDB.Models;
using PoohAPI.Logic.Common;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Companies.Init
{
    public class CompanyMappingProfile : Profile
    {
        public CompanyMappingProfile()
        {
            CreateMap<DBCompany, Location>()
                .ForMember(d => d.CountryName, o => o.MapFrom(s => s.land_naam))
                .ForMember(d => d.CountryId, o => o.MapFrom(s => s.bedrijf_vestiging_land))
                .ForMember(d => d.City, o => o.MapFrom(s => s.bedrijf_vestiging_plaats))
                .ForMember(d => d.ZipCode, o => o.MapFrom(s => s.bedrijf_vestiging_postcode))
                .ForMember(d => d.Street, o => o.MapFrom(s => s.bedrijf_vestiging_straat))
                .ForMember(d => d.HouseNumber, o => o.MapFrom(s => s.bedrijf_vestiging_huisnr))
                .ForMember(d => d.HouseNumberAdditions, o => o.MapFrom(s => s.bedrijf_vestiging_toev))
                .ForMember(d => d.Latitude, o => o.MapFrom(s => (double)s.bedrijf_breedtegraad))
                .ForMember(d => d.Longitude, o => o.MapFrom(s => (double)s.bedrijf_lengtegraad))
                .ForMember(d => d.Range, o => o.MapFrom(s => s.distance))
                .ReverseMap();

            CreateMap<DBCompany, Company>()
                .IncludeBase<DBCompany, BaseCompany>()
                .ForMember(d => d.SocialLinkLinkedin, o => o.MapFrom(s => s.bedrijf_social_linkedin))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.bedrijf_beschrijving))
                .ForMember(d => d.EmailAddress, o => o.MapFrom(s => s.bedrijf_contactpersoon_email))
                .ForMember(d => d.Website, o => o.MapFrom(s => s.bedrijf_website))
                .ReverseMap();

            CreateMap<DBCompany, BaseCompany>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.bedrijf_id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.bedrijf_handelsnaam))
                .ForMember(d => d.Location, o => o.MapFrom(s => s))
                .ForMember(d => d.LogoPath, o => o.MapFrom(s => s.bedrijf_logo))
                .ForMember(d => d.AverageReviewStars, o => o.MapFrom(s => (double)s.average_reviews))
                .ForMember(d => d.Majors, o => o.MapFrom(s => s.opleidingen))
                .ReverseMap();

            CreateMap<IDataReader, DBCompany>().ConvertUsing<DataReaderTypeConverter<DBCompany>>();
        }
    }
}
