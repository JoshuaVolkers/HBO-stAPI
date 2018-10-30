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
        ClaimsIdentity CreateClaimsIdentity(bool activeUser, int userId, string userRole);
        JWTToken GenerateJWT(ClaimsIdentity user, string refreshToken, int expiryTimeInSeconds = 3600);
    }
}
