using AutoMapper;
using PoohAPI.Infrastructure.CompanyDB.Models;
using PoohAPI.Logic.Common;
using PoohAPI.Logic.Common.Models;
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
                .ReverseMap();

            CreateMap<DBCompany, Company>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.bedrijf_id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.bedrijf_handelsnaam))
                .ForMember(d => d.Location, o => o.MapFrom(s => s))
                .ForMember(d => d.LogoPath, o => o.MapFrom(s => s.bedrijf_logo))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.bedrijf_beschrijving))
                .ForMember(d => d.EmailAddress, o => o.MapFrom(s => s.bedrijf_contactpersoon_email))
                .ForMember(d => d.Website, o => o.MapFrom(s => s.bedrijf_website))
                .ReverseMap();
            
            CreateMap<IDataReader, DBCompany>().ConvertUsing<DataReaderTypeConverter<DBCompany>>();
        }
    }
}
