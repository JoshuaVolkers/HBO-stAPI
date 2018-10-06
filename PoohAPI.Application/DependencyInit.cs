using Microsoft.Extensions.DependencyInjection;

namespace PoohAPI.Application
{
    public static class DependencyInit
    {
        public static void Init(IServiceCollection services)
        {          
            Logic.Users.Init.DIComponentsConfig.RegisterComponents(services);
        }
    }
}
