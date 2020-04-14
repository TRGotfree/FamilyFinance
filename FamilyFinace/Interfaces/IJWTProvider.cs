using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FamilyFinace.Interfaces
{
    public interface IJWTProvider
    {
        JwtSecurityToken GetToken(ClaimsIdentity claims);
        string WriteToken(JwtSecurityToken jwtSecurityToken);
    }
}
