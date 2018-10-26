using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Security.Principal;
using PoohAPI.Models;
using Microsoft.Extensions.Configuration;

namespace PoohAPI.Authorization
{
    public class TokenHelper : ITokenHelper
    {
        private readonly IConfiguration _configSettings;
        public TokenHelper(IConfiguration configSettings)
        {
            _configSettings = configSettings;
        }

        private string RequestToken(ClaimsIdentity user, int expiryTimeInSeconds)
        {
            var claims = new List<Claim>()
            {
                user.FindFirst("id"),
                user.FindFirst(ClaimTypes.Name),
                user.FindFirst(JwtRegisteredClaimNames.Iat),
                user.FindFirst(ClaimTypes.Role)
            };            

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configSettings.GetValue<string>("JWTSigningKey")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configSettings.GetValue<string>("JWTIssuer"),
                audience: _configSettings.GetValue<string>("JWTAudience"),
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(expiryTimeInSeconds),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsIdentity CreateClaimsIdentity(string userName, int userId, string userRole)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim("id", userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                new Claim(ClaimTypes.Role, userRole)
            });
        }

        public JWTToken GenerateJWT(ClaimsIdentity user, string refreshToken, int expiryTimeInSeconds = 3600)
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

