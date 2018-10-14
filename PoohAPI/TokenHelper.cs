using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using PoohAPI.Logic.Common.Models;
using System.Security.Claims;
using System.Text;

namespace PoohAPI
{
    public static class TokenHelper
    {
        //public static string RequestToken(User user)
        //{
        //    user.Roles = new[] {"bla", "cs_candidate"};
        //    var claims = new List<Claim>() {new Claim(ClaimTypes.Name, user.NiceName), new Claim(ClaimTypes.Sid, user.Id.ToString())};
        //    if (user.Roles != null && user.Roles.Length != 0)
        //    {
        //        foreach (var role in user.Roles)
        //        {
        //            claims.Add(new Claim(ClaimTypes.Role, role));
        //        }
        //    }                 

        //    //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTSigningToken"]));
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("D2n2skmv8xY3ZcSzgc9eMwWjYzXMPXHtWKarHxscXeZN6FbX6qkeBsw88txVPRyHf4j2VkEH4XZLskGgKSJHHybhjVXAHXEYMw8z6gGTG58wT8y49bJ8ezMJNXhFz9Vd"));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: "poohapi",
        //        audience: "poohapi",
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddMinutes(2),
        //        signingCredentials: creds);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        public static bool ValidateToken(string token, string issuer)
        {


            return true;
        }
    }
}

