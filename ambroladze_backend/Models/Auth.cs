﻿using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

namespace ambroladze_backend.Models
{
    internal class Auth
    {
        public static string Issuer => "VA";
        public static string Audience => "APIclients";
        public static int LifetimeInYears => 1;
        public static SecurityKey SigningKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes("qwertyuiop0987654321asdfghjkl,mnbvcxz"));

        internal static object GenerateToken(Client client)
        {
            var now = DateTime.UtcNow;
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, "user"),
                    //new Claim(ClaimsIdentity.DefaultRoleClaimType, is_worker?"worker":"guest"),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, client.IsAdmin?"admin":"guest"),
                    new Claim("id", client.Id.ToString())
                };
            ClaimsIdentity identity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: Issuer,
                    audience: Audience,
                    notBefore: now,
                    expires: now.AddYears(LifetimeInYears),
                    claims: identity.Claims,
                    signingCredentials: new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256)); ;
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new { token = encodedJwt };
        }
    }
}

