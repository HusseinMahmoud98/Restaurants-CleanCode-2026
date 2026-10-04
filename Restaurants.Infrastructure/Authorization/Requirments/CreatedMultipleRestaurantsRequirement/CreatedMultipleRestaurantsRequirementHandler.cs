using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Infrastructure.Authorization.Requirments.CreatedMultipleRestaurantsRequirement
{
    internal class CreatedMultipleRestaurantsRequirementHandler(
        ILogger<CreatedMultipleRestaurantsRequirementHandler> _logger, 
        IUserContext _userContext,
        IRestaurantsRepository _restaurantsRepository)
        : AuthorizationHandler<CreatedMultipleRestaurantsRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsRequirement requirement)
        {
            var currentUser = _userContext.GetCurrentUser()
                ?? throw new UnauthenticatedException();

            var restaurants = await _restaurantsRepository.GetAllAsync();
            var userRestaurantsCreatedCount = restaurants.Count(r => r.OwnerId == currentUser.Id);

            _logger.LogInformation("User: {Email}, number of restaurants: {NumberOfRestaurants} - Handling MinimumNumberRestaurantsRequirement",
                currentUser.Email, requirement.MinimumNumberRestaurants);
            if (userRestaurantsCreatedCount >= requirement.MinimumNumberRestaurants)
            {
                _logger.LogInformation("Authorization succeeded");
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogWarning("Authorization failed: user has {NumberOfRestaurants} restaurants, but requires at least {MinimumNumberRestaurants}",
                    userRestaurantsCreatedCount, requirement.MinimumNumberRestaurants);
                context.Fail();
            }
        }
    }
}
