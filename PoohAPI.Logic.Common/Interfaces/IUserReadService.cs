using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.BaseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Interfaces
{
    public interface IUserReadService
    {
        IEnumerable<User> GetAllUsers(int maxCount, int offset, string educationalAttainment = null, 
            string educations = null, string cityName = null, string countryName = null, int? range = null,
            string additionalLocationSearchTerms = null, int? preferredLanguage = null);
        User GetUserById(int id, bool isActive = true);
        T GetUserByEmail<T>(string email);
        JwtUser Login(string email = null, string password = null, int? userid = null);
        JwtUser GetUserByRefreshToken(string refreshToken);
        UserEmailVerification GetUserEmailVerificationByUserId(int userId);
        UserEmailVerification GetUserEmailVerificationByToken(string token);
    }
}
