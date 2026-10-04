using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommandHandler(ILogger<DeleteRestaurantCommandHandler> _logger,
        IRestaurantsRepository _restaurantsRepository,
        IRestaurantAuthorizationEvaluator _restaurantAuthorizationService)
        : IRequestHandler<DeleteRestaurantCommand>
    {
        public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Deleting restaurant with id: {RestaurantId}", request.Id);

            var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

            if (!_restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
                throw new ForbidException();
            

            await _restaurantsRepository.DeleteAsync(restaurant);
            _logger.LogInformation("Restaurant with id {RestaurantId} deleted", request.Id.ToString());
        }
    }
}
