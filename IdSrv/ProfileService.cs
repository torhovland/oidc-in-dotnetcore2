using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace IdSrv
{
    public class ProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.IssuedClaims.Add(context.Subject.FindFirst(c => c.Type == JwtClaimTypes.Name));
            context.IssuedClaims.Add(context.Subject.FindFirst(c => c.Type == JwtClaimTypes.Email));
            context.IssuedClaims.Add(context.Subject.FindFirst(c => c.Type == "favorittfarge"));
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}