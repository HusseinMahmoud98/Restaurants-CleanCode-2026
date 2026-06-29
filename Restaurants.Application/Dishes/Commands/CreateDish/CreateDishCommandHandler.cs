using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.CreateDish
{
    public class CreateDishCommandHandler(IMapper _mapper,
        IDishesRepository _dishesRepository,
        ILogger<CreateDishCommandHandler> _logger,
        IRestaurantsRepository _restaurantsRepository)
        : IRequestHandler<CreateDishCommand, int>
    {
        public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating new dish: {@DishRequest}", request);

            var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
                 ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());


            var dish = _mapper.Map<Dish>(request);
            return await _dishesRepository.CreateAsync(dish); //return the id of the dish
        }
    }
}
