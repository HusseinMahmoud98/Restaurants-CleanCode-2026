using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirments.CreatedMultipleRestaurantsRequirement
{
    public class CreatedMultipleRestaurantsRequirement(int minimumRestaurantsCreated) : IAuthorizationRequirement
    {
        public int MinimumNumberRestaurants { get; } = minimumRestaurantsCreated;
    }
}
