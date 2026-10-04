using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.AuthorizationEvaluators
{
    public class RestaurantAuthorizationEvaluator(ILogger<RestaurantAuthorizationEvaluator> _logger, IUserContext _userContext)
        : IRestaurantAuthorizationEvaluator
    {
        public bool Authorize(Restaurant restaurant, ResourceOperation resourceOperation)
        {
            var currentUser = _userContext.GetCurrentUser()
                ?? throw new UnauthenticatedException();

            _logger.LogInformation("Authorizing user {UserEmail} to operation {Operation} for restaurant {RestaurantName}", currentUser.Email, resourceOperation, restaurant.Name);

            if (resourceOperation == ResourceOperation.Read || resourceOperation == ResourceOperation.Create)
            {
                _logger.LogInformation("Create/Read operation - successful authorization");
                return true;
            }

            if (resourceOperation == ResourceOperation.Delete && currentUser.IsInRole(UserRoles.Admin))
            {
                _logger.LogInformation("Admin user, delete operation - successful authorization");
                return true;
            }

            if ((resourceOperation == ResourceOperation.Delete || resourceOperation == ResourceOperation.Update) 
                && restaurant.OwnerId == currentUser.Id)
            {
                _logger.LogInformation("Restaurant owner, delete/update operation - successful authorization");
                return true;
            }

            return false;
        }
    }
}
