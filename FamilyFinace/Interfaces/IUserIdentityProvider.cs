using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FamilyFinace.Interfaces
{
    public interface IUserIdentityProvider
    {
        ClaimsIdentity GetIdentity(string userLogin);
    }
}
