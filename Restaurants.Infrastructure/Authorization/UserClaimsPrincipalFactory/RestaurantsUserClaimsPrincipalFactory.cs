using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using System.Security.Claims;

namespace Restaurants.Infrastructure.Authorization.UserClaimsPrincipalFactory
{
    public class RestaurantsUserClaimsPrincipalFactory(UserManager<User> userManager, 
        RoleManager<IdentityRole> roleManager,
        IOptions<IdentityOptions> options)
        : UserClaimsPrincipalFactory<User, IdentityRole>(userManager, roleManager, options)
    {
        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var claimsIdentity = await GenerateClaimsAsync(user);

            if (user.Nationality is not null)
            {
                claimsIdentity.AddClaim(new Claim(CustomClaimTypes.Nationality, user.Nationality));
            }

            if (user.DateOfBirth is not null)
            {
                claimsIdentity.AddClaim(new Claim(CustomClaimTypes.DateOfBirth, user.DateOfBirth.Value.ToString("yyyy-MM-dd")));
            }

            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}
