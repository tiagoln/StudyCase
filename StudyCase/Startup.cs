using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Core.Interfaces;
using Core.Model;
using Infrastructure;
using Infrastructure.Database;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using StudyCase.ActionFilters;
using StudyCase.Services;

namespace StudyCase
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // ===== Add DbContext ========
            services.AddDbContext<StudyContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // ===== Add Identity ========
            var builder = services.AddIdentityCore<User>(options =>
                {
                    // === Password settings ===
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequiredUniqueChars = 2;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;

                    // === Lockout settings ===
                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;

                    // === Signin settings ===
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;

                    // === User settings ===
                    options.User.RequireUniqueEmail = true;
                }
            );
            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder
                .AddUserStore<NewPcsUserStore>()
                .AddRoleStore<NewPcsRoleStore>()
                .AddRoleValidator<RoleValidator<Role>>()
                .AddRoleManager<RoleManager<Role>>()
                .AddSignInManager<SignInManager<User>>();

            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["Tokens:JwtIssuer"],
                        ValidAudience = Configuration["Tokens:JwtIssuer"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:JwtKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.AddMvc(options =>
            {
                // ===== Add Action Filters ========
                options.Filters.Add(typeof(TransactionFilter));
                options.Filters.Add(typeof(ExceptionFilter));
            });

            // ===== Add Dependency Injection stuff ========
            services.AddScoped<IUnityOfWork, UnitOfWork>();
            services.AddTransient<JwtTokenGenerator>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // ===== Add Routing ========
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // ===== Use Identity Authentication ========
            app.UseAuthentication();

            app.UseMvc(builder => { builder.MapRoute("default", "api/{controller}/{action}"); });
        }
    }
}