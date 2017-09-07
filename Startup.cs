using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NoteShareAPI.Entities;

namespace NoteShareAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment _env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();

            services.AddDbContext<NoteContext>(options => options.UseOpenIddict());

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<NoteContext>()
                .AddDefaultTokenProviders();
                
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            // Register the OpenIddict services.
            services.AddOpenIddict(options =>
            {
                // Register the Entity Framework stores.
                options.AddEntityFrameworkCoreStores<NoteContext>();

                // Register the ASP.NET Core MVC binder used by OpenIddict.
                // Note: if you don't call this method, you won't be able to
                // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                options.AddMvcBinders();

                // Enable the token endpoint.
                options.EnableTokenEndpoint("/connect/token");

                // Enable the password flow.
                options.AllowPasswordFlow();

                // During development, you can disable the HTTPS requirement.
                if (_env.IsDevelopment())
                    options.DisableHttpsRequirement();
            });

            if (_env.IsProduction())
            {
                services.Configure<MvcOptions>(options =>
                {
                    options.Filters.Add(new RequireHttpsAttribute());
                });
            }

            services.AddAuthentication()
                .AddOAuthValidation();

            services.AddScoped<IDbInitialize, DbInitialize>();
            
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDbInitialize initializer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                var options = new RewriteOptions().AddRedirectToHttps();
                app.UseRewriter(options);
            }

            app.UseAuthentication();
            app.UseMvc();

            app.UseFileServer();

            initializer.Seed();
        }
    }
}
