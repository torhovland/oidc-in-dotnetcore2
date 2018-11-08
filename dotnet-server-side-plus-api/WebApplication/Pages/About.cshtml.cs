using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApi.Controllers;

namespace WebApplication.Pages
{
    public class AboutModel : PageModel
    {
        public string Namn { get; set; }
        public string Brukarnamn { get; set; }
        public string Epostadresse { get; set; }
        public string Favorittfarge { get; set; }

        public async Task OnGet()
        {
            var user = HttpContext.User;
            var claims = user.Claims.ToList();

            Namn = user.Identity.Name;
            Brukarnamn = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            Epostadresse = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
            Favorittfarge = claims.FirstOrDefault(c => c.Type == "favorittfarge")?.Value;

            var token = await HttpContext.GetTokenAsync("access_token");
            Response.Cookies.Append("access-token", token);
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            try {
                HttpContext.Response.Cookies.Delete("access-token");
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
            } catch (InvalidOperationException) {
                // No contact with identity server. Just delete the browser cookie instead.
                HttpContext.Response.Cookies.Delete("auth");
            }

            return Redirect("/");
        }
    }
}
