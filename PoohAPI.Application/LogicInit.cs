using Unity;

namespace PoohAPI.Application
{
    //TODO: Find out the best way to implement DI. 
    public static class LogicInit
    {
        public static void Init(IUnityContainer container)
        {
            Logic.Users.Init.UnityComponentsConfig.RegisterComponents(container);
        }
    }
}
