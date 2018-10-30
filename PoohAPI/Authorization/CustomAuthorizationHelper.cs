using System;
using System.Linq;
using System.Security.Claims;

namespace PoohAPI.Authorization
{
    public static class CustomAuthorizationHelper
    {
        public static int GetCurrentUserId(ClaimsPrincipal user)
        {
            Int32.TryParse(user.Claims.SingleOrDefault(c => c.Type == "id").Value, out int userId);
            return userId;
        }

        public static bool GetCurrentUserActive(ClaimsPrincipal user)
        {
            Boolean.TryParse(user.Claims.SingleOrDefault(c => c.Type == "active").Value, out bool userIsActive);
            return userIsActive;
        }
    }
}
