using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication.Pages
{
    public class AboutModel : PageModel
    {
        public string Namn { get; set; }
        public string Brukarnamn { get; set; }
        public string Epostadresse { get; set; }
        public string Favorittfarge { get; set; }

        public void OnGet()
        {
            var user = HttpContext.User;
            var claims = user.Claims.ToList();

            Namn = user.Identity.Name;
            Brukarnamn = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            Epostadresse = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;
            Favorittfarge = claims.FirstOrDefault(c => c.Type == "favorittfarge")?.Value;
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            try {
                await HttpContext.SignOutAsync("Cookies");
                await HttpContext.SignOutAsync("oidc");
            } catch (InvalidOperationException) {
                // No contact with identity server. Just delete the browser cookie instead.
                HttpContext.Response.Cookies.Delete("auth");
            }

            return Redirect("/");
        }
    }
}
