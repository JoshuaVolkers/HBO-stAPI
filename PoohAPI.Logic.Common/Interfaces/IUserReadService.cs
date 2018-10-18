using PoohAPI.Logic.Common.Models;
using PoohAPI.Logic.Common.Models.BaseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoohAPI.Logic.Common.Interfaces
{
    public interface IUserReadService
    {
        IEnumerable<BaseUser> GetAllUsers(int maxCount, int offset, string educationalAttainment = null, 
            string educations = null, string cityName = null, string countryName = null, int? range = null,
            string additionalLocationSearchTerms = null, int? preferredLanguage = null);
        User GetUserById(int id);
        User GetUserByEmail(string email);
        User Login(string email, string password);
    }
}
