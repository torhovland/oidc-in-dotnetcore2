using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication
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
            // Remove the default non-OIDC claims, otherwise the OIDC claims will not be added properly.
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    // Always try to use an existing login session stored in a cookie first
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                    // If that fails, use OpenID Connect to log the user on.
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    options.Cookie.Name = "auth";
                })
                .AddOpenIdConnect(options =>
                {
                    const string favorittfarge = "favorittfarge";

                    options.Authority = Configuration["Oidc:Authority"];
                    options.ClientId = Configuration["Oidc:ClientId"];
                    options.ClientSecret = Configuration["Oidc:ClientSecret"];

                    options.ResponseType = "code";

                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    options.Scope.Add(favorittfarge);

                    // Pull the user's claims from the userinfo endpoint on the identity server
                    options.GetClaimsFromUserInfoEndpoint = true;

                    // Need to specify any custom claims that we want to pull into HttpContext.User.Claims
                    options.ClaimActions.MapJsonKey(favorittfarge, favorittfarge);

                    // This will fix HttpContext.User.Identity.Name and role checks
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role",
                    };
                });

            services
                .AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizePage("/about");
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
