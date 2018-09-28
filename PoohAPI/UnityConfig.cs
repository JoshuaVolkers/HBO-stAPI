using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace PoohAPI
{
    public static class UnityConfig
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            Logic.Common.UnityConfig.RegisterTypes(container);
        }
    }
}
