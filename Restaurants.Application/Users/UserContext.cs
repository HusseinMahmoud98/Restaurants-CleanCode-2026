using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Users
{
    public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
    {
        public CurrentUser? GetCurrentUser()
        {
            var user = httpContextAccessor?.HttpContext?.User;

            //This enforces that the method should only be called when a user context exists.
            if (user is null) throw new InvalidOperationException("User context is not present");

            //This prevents treating anonymous users as valid.
            if (user.Identity is null || !user.Identity.IsAuthenticated) return null;

            //Note: Each claim has a type and a value. So we filter by the claim type and then select the value.
            var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var email = user.FindFirst(c => c.Type == ClaimTypes.Email)!.Value;
            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            var nationality = user.FindFirstValue(CustomClaimTypes.Nationality);
            
            var dateOfBirthString = user.FindFirst(c => c.Type == CustomClaimTypes.DateOfBirth)?.Value;
            var dateOfBirth = dateOfBirthString == null ?
                (DateOnly?)null :
                DateOnly.ParseExact(dateOfBirthString, "yyyy-MM-dd");

            return new CurrentUser(userId, email, roles, nationality, dateOfBirth);
        }
    }
}
