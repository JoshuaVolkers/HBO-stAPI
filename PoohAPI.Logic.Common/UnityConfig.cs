using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Unity;
using Unity.RegistrationByConvention;

namespace PoohAPI.Logic.Common
{
    public static class UnityConfig
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterTypes(
                AllClasses.FromLoadedAssemblies()
                    .Where(c => c.Namespace.StartsWith("PoohAPI") && c.BaseType != typeof(Profile)),
                WithMappings.FromMatchingInterface,
                WithName.Default,
                WithLifetime.Transient
            );
        }
    }
}
