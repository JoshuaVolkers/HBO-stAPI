using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using PoohAPI.Logic.Common.Models;
using System.Security.Claims;
using System.Text;
using System.Security.Principal;
using Newtonsoft.Json;
using PoohAPI.Models;
//Test

namespace PoohAPI
{
    public static class TokenHelper
    {
        private static string RequestToken(ClaimsIdentity user, int expiryTimeInSeconds)
        {
            var claims = new List<Claim>()
            {
                user.FindFirst("id"),
                user.FindFirst(ClaimTypes.Name),
                user.FindFirst(JwtRegisteredClaimNames.Iat),
                user.FindFirst(ClaimTypes.Role)
            };            

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("D2n2skmv8xY3ZcSzgc9eMwWjYzXMPXHtWKarHxscXeZN6FbX6qkeBsw88txVPRyHf4j2VkEH4XZLskGgKSJHHybhjVXAHXEYMw8z6gGTG58wT8y49bJ8ezMJNXhFz9Vd"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "poohapi",
                audience: "poohapi",
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(expiryTimeInSeconds),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static ClaimsIdentity CreateClaimsIdentity(string userName, int userId, string userRole)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim("id", userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                new Claim(ClaimTypes.Role, userRole)
            });
        }

        public static JWTToken GenerateJWT(ClaimsIdentity user, string refreshToken, int expiryTimeInSeconds = 3600)
        {
            var response = new JWTToken
            (
                user.Claims.SingleOrDefault(c => c.Type == "id").Value,
                RequestToken(user, expiryTimeInSeconds),
                expiryTimeInSeconds,
                refreshToken
            );
            return response;
        }
    }
}

