using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using wishApp.Api.Data;
using wishApp.Api.Models;

namespace wishApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // services.AddIdentity<User, IdentityRole>(cfg => {
            //     cfg.User.RequireUniqueEmail = true;
            //     cfg.Password.RequireUppercase = false;
            //     cfg.Password.RequireDigit = false;
            //     cfg.Password.RequireNonAlphanumeric = false;
            //     cfg.Password.RequiredLength = 4;
            // }).AddEntityFrameworkStores<AppDbContext>();
            IdentityBuilder builder = services.AddIdentityCore<User>(opt => {
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<AppDbContext>();
            // builder.AddRoleValidator<RoleValidator<Role>>();
            // builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            services
                .AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddCookie(IdentityConstants.ApplicationScheme);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = false,
                       ValidateAudience = false,
                        // ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                       ValidIssuer = Configuration["Jwt:Issuer"],
                       ValidAudience = Configuration["Jwt:Issuer"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                   };
               });
            services.AddCors();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // ?
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWishRepository, WishRepository>();

            services.AddDbContext<AppDbContext>(x => x.UseSqlite("Data Source=payAppFin.db"));
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(
                    options => options.SerializerSettings.ReferenceLoopHandling
                        = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseHsts();
            }
            app.UseCors(opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseAuthentication();
            // app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
