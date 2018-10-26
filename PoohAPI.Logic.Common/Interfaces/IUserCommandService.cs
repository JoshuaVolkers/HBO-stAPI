using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PoohAPI.Logic.Common.Models.InputModels;

namespace PoohAPI.Logic.Common.Interfaces
{
    public interface IUserCommandService
    {
        JwtUser RegisterUser(string login, string email, UserAccountType accountType, string password = null);
        void ResetPassword(string email, int userid);
        User UpdateUser(UserUpdateInput userInput);
        void DeleteUser(int id);
        string CreateEmailVerificationToken(int userId);
        void DeleteEmailVerificationToken(int userId);
        void CreateEmailVerification(int userId, string token, DateTime expirationDate);
        User VerifyUserEmail(string token);
        string VerifyResetPassword(string token);
        void DeleteRefreshToken(string refreshToken);
        string UpdateRefreshToken(int userId);
        bool UpdatePassword(int userid, PasswordUpdateInput passwordUpdateInput, bool isreset = false);
    }
}
