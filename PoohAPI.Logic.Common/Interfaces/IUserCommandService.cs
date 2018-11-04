using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Interfaces
{
    public interface IUserCommandService
    {
        JwtUser RegisterUser(string login, string email, UserAccountType accountType, string password = null);
        void ResetPassword(string email, int userid);

        User UpdateUser(int countryId, string city, int educationId, int educationalAttainmentId,
            int preferredLanguageId, int userId, string AdditionalLocationIdentifier = null);
        void DeleteUser(int id);
        string CreateEmailVerificationToken(int userId);
        void DeleteEmailVerificationToken(int userId);
        void CreateEmailVerification(int userId, string token, DateTime expirationDate);
        User VerifyUserEmail(string token);
        string VerifyResetPassword(string token);
        void DeleteRefreshToken(string refreshToken);
        string UpdateRefreshToken(int userId);
        bool UpdatePassword(int userid, string newPassword, string oldPassword = null, bool isreset = false);
        User CreateUserVerification(int createdUserId);
    }
}
