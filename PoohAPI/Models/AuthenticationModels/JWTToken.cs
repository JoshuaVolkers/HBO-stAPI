using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoohAPI.Models
{
    public class JWTToken
    {
        public JWTToken(string id, string authToken, int expiresIn, string refreshToken)
        {
            Id = id;
            AuthToken = authToken;
            ExpiresIn = expiresIn;
            RefreshToken = refreshToken;
        }

        public string Id { get; }
        public string AuthToken { get; }
        public int ExpiresIn { get; }
        public string RefreshToken { get; }
    }
}
