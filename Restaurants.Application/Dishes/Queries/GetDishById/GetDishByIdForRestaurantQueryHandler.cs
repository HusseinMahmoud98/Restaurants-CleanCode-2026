using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Queries.GetDishById
{
    public class GetDishByIdForRestaurantQueryHandler(IRestaurantsRepository _restaurantsRepository,
        IMapper _mapper,
        ILogger<GetDishByIdForRestaurantQueryHandler> _logger)
        : IRequestHandler<GetDishByIdForRestaurantQuery, DishDto>
    {
        public async Task<DishDto> Handle(GetDishByIdForRestaurantQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving dish with id: {DishId} from restaurant with id: {RestaurantId}...", request.DishId, request.RestaurantId);

            //From my point of view, getting the whole restaurant is not the best practice but rather check if it exists or not then get only the required dish
            var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
                 ?? throw new NotFoundException(nameof(Restaurant), request.DishId.ToString());

            var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == request.DishId)
                ?? throw new NotFoundException(nameof(Dish), request.DishId.ToString());

            return _mapper.Map<DishDto>(dish);
        }
    }
}
