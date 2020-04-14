using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyFinace.Services
{
    public class AuthentificationOptionsProvider
    {
        private IWebHostEnvironment environment;

        public AuthentificationOptionsProvider(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public SecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        }

        public string Issuer => Environment.GetEnvironmentVariable("JWT_ISSUER");

        public string Audience => Environment.GetEnvironmentVariable("JWT_AUDIENCE");

        public string Key => Environment.GetEnvironmentVariable("JWT_KEY");

        public int LifeTime
        {
            get
            {
                if (environment.IsDevelopment())
                    return 60; //В минутах
                else
                    return int.Parse(Environment.GetEnvironmentVariable("JWT_LIFETIME"));
            }
        }

    }
}
