using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Infrastructure.Authorization.Requirments.MinimumAgeRequirement
{
    internal class MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> _logger, IUserContext _userContext)
        : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            var currentUser =  _userContext.GetCurrentUser()
                ?? throw new UnauthenticatedException();
            
            _logger.LogInformation("User: {Email}, date of birth: {DoB} - Handling MinimumAgeRequirement", 
                currentUser.Email, currentUser.DateOfBirth);

            if (currentUser.DateOfBirth is null)
            {
                _logger.LogWarning("User date of birth is null");
                context.Fail();

                return Task.CompletedTask;
            }

            if (currentUser.DateOfBirth.Value.AddYears(requirement.MinimumAge) <= DateOnly.FromDateTime(DateTime.Today))
            {
                _logger.LogInformation("Authorization succeeded");
                context.Succeed(requirement);
            }

            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
