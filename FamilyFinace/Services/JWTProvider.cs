using FamilyFinace.Interfaces;
using FamilyFinace.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FamilyFinace.Services
{
    public class JWTProvider : IJWTProvider
    {
        private AuthentificationOptionsProvider authOptionsProvider;

        public JWTProvider(AuthentificationOptionsProvider authentificationOptions)
        {
            authOptionsProvider = authentificationOptions;           
        }

        public JwtSecurityToken GetToken(ClaimsIdentity claims)
        {
            DateTime nowDateTime = DateTime.UtcNow;
            DateTime expirationDateTime = nowDateTime.Add(TimeSpan.FromMinutes(authOptionsProvider.LifeTime));

            var newToken = new JwtSecurityToken(
                issuer: authOptionsProvider.Issuer,
                audience: authOptionsProvider.Audience,
                notBefore: nowDateTime,
                claims: claims.Claims,
                expires: expirationDateTime,
                signingCredentials: new SigningCredentials(authOptionsProvider.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return newToken;
        }

        public string WriteToken(JwtSecurityToken jwtSecurityToken)
        {
            if (jwtSecurityToken == null)
                throw new ArgumentNullException(nameof(jwtSecurityToken));

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
