using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Modelo.Domain.Entities;
using Modelo.Domain.Interfaces;
using Modelo.Infra.Data.Context;
using Modelo.Infra.Data.Repository;
using Modelo.Service.Services;
using System;
using System.Text;

namespace Modelo.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Environment.SetEnvironmentVariable("SecretKey", AuthorizationService.GenerateKey());

            services.AddMvc();

            services.AddScoped<IRepository<UserEntity>, BaseRepository<UserEntity>>();
            services.AddScoped<IService<UserEntity>, UserService<UserEntity>>();

            services.AddScoped<DbContext, MemoryContext>();
            services.AddDbContext<MemoryContext>(opt => opt.UseInMemoryDatabase("DataUserMemory"));

            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("SecretKey"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
