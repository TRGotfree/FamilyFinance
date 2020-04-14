using FamilyFinace.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FamilyFinace.Services
{
    public class UserIdentityProvider : IUserIdentityProvider
    {
        public ClaimsIdentity GetIdentity(string userLogin)
        {
            if (string.IsNullOrEmpty(userLogin))
                throw new ArgumentNullException(nameof(userLogin));

            try
            {
                var claims = new List<Claim> {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, userLogin)
                    };

                return new ClaimsIdentity(claims, "JWToken", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
