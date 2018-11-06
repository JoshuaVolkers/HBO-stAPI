using PoohAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PoohAPI.Authorization
{
    public interface ITokenHelper
    {
        ClaimsIdentity CreateClaimsIdentity(bool activeUser, int userId, string userRole, string refreshToken);
        string GenerateJWT(ClaimsIdentity user, int expiryTimeInSeconds = 0);
    }
}
