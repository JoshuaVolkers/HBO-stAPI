using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Enums;

namespace PoohAPI.Logic.Common.Interfaces
{
    public interface IUserCommandService
    {
        User RegisterUser(string login, string email, UserAccountType accountType, string password = null);
    }
}
