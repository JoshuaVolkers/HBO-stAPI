using PoohAPI.Logic.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Interfaces
{
    public interface IUserCommandService
    {
        User RegisterUser(string login, string password, string email);
    }
}
