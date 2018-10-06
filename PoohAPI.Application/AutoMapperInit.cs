using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Data;
using PoohAPI.Logic.Users.Init;

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
            });
        }
    }
}
