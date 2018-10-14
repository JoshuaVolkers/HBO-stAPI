using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.InputModels;

namespace PoohAPI.Logic.Common.Interfaces
{
    public interface IUserCommandService
    {
        User UpdateUser(UserUpdateInput userInput);
        void DeleteUser(int id);
    }
}
