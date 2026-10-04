using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.CreateDish
{
    public class CreateDishCommandHandler(IMapper _mapper,
        IDishesRepository _dishesRepository,
        ILogger<CreateDishCommandHandler> _logger,
        IRestaurantsRepository _restaurantsRepository,
        IRestaurantAuthorizationEvaluator _restaurantAuthorizationService
        )
        : IRequestHandler<CreateDishCommand, int>
    {
        public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating new dish: {@DishRequest}", request);

            var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
                 ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());


            if (!_restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
                throw new ForbidException();

            var dish = _mapper.Map<Dish>(request);
            return await _dishesRepository.CreateAsync(dish); //return the id of the dish
        }
    }
}
