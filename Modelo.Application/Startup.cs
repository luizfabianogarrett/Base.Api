using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Modelo.Domain.Entities;
using Modelo.Domain.Interfaces;
using Modelo.Domain.Models;
using Modelo.Infra.Data.Context;
using Modelo.Infra.Data.Repository;
using Modelo.UserServive.Services;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Globalization;
using System.IO;
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
            var settings = new AppSettings();
            new ConfigureFromConfigurationOptions<AppSettings>(Configuration.GetSection("AppSettings")).Configure(settings);

            Environment.SetEnvironmentVariable("SecretKey", settings.Salt);

            services.AddMvc();

            services.AddScoped<IRepository<UserEntity>, BaseRepository<UserEntity>>();

            services.AddScoped<IService<UserEntity>, BaseService<UserEntity>>();
            services.AddScoped<IUserService<UserEntity>, UserService<UserEntity>>();

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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "User API", Version = "v1" });

                string pathApp = PlatformServices.Default.Application.ApplicationBasePath;
                string nameApp = PlatformServices.Default.Application.ApplicationName;
                string pathXmlDoc = Path.Combine(pathApp, $"{nameApp}.xml");

                c.IncludeXmlComments(pathXmlDoc);
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

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API V1");
                c.RoutePrefix = string.Empty;
            });

            var supportedCultures = new[] { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
        }
    }
}
