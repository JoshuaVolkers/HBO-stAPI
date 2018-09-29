using PoohAPI.Infrastructure.UserDB.Repositories;
using PoohAPI.Logic.Common.Interfaces;
using PoohAPI.Logic.Users.Services;
using Unity;
using Unity.Lifetime;

namespace PoohAPI.Logic.Users.Init
{
    public static class UnityComponentsConfig
    {
        public static void RegisterComponents(IUnityContainer container)
        {
            container.RegisterType<IUserReadService, UserReadService>(new TransientLifetimeManager());
            container.RegisterType<IUserRepository, UserRepository>(new TransientLifetimeManager());
        }
    }
}
