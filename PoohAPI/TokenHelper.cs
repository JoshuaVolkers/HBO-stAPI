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

namespace PoohAPI
{
    public static class TokenHelper
    {
        public static string RequestToken(ClaimsIdentity user)
        {
            //user.Roles = new[] {"bla", "cs_candidate"};
            var claims = new List<Claim>()
            {
                user.FindFirst("id"),
                user.FindFirst(ClaimTypes.Name)
            };
            //if (user != null && user.Roles != null && user.Roles.Length != 0)
            //{
            //    foreach (var role in user.Roles)
            //    {
            //        claims.Add(new Claim(ClaimTypes.Role, role));
            //    }
            //}                 

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("D2n2skmv8xY3ZcSzgc9eMwWjYzXMPXHtWKarHxscXeZN6FbX6qkeBsw88txVPRyHf4j2VkEH4XZLskGgKSJHHybhjVXAHXEYMw8z6gGTG58wT8y49bJ8ezMJNXhFz9Vd"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "poohapi",
                audience: "poohapi",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static ClaimsIdentity CreateClaimsIdentity(string userName, int userId)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim("id", userId.ToString())
            });
        }

        public static JWTToken GenerateJWT(ClaimsIdentity user)
        {
            var response = new JWTToken
            (
                user.Claims.SingleOrDefault(c => c.Type == "id").Value,
                RequestToken(user),
                600
            );

            return response;
        }

        public static bool ValidateToken(string token, string issuer)
        {


            return true;
        }
    }
}

