using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyFinace.Interfaces;
using FamilyFinace.Repository;
using FamilyFinace.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace FamilyFinace
{
    public class Startup
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Environment.GetEnvironmentVariable("CONN_STRING");

            //services.AddControllers();

            if (hostingEnvironment.IsDevelopment())
                services.AddCors(policy => policy.AddPolicy("DevPolicy", policyBuilder =>
                {
                    policyBuilder
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowAnyHeader();
                }));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       var authOptions = new AuthentificationOptionsProvider(hostingEnvironment);

                       options.RequireHttpsMetadata = !hostingEnvironment.IsDevelopment();

                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidateIssuer = true,
                           ValidIssuer = authOptions.Issuer,
                           ValidateAudience = true,
                           ValidAudience = authOptions.Audience,
                           ValidateLifetime = true,
                           IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                           ValidateIssuerSigningKey = true
                       };
                   });

            services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(connectionString));
            services.AddTransient<AuthentificationOptionsProvider>();
            services.AddTransient<IRepository, RepositoryProvider>();
            services.AddTransient<ITransformer, ModelTransformer>();
            services.AddTransient<IHashGenerator, HashGenerator>();
            services.AddTransient<ICustomLogger, CustomLogger>();
            services.AddTransient<IJWTProvider, JWTProvider>();
            services.AddTransient<IUserIdentityProvider, UserIdentityProvider>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("DevPolicy");
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseDefaultFiles();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index", "Index");
            });
        }
    }
}
