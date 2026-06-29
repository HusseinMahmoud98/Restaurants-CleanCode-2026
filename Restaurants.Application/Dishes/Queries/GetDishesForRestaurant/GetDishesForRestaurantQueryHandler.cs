using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;


namespace Restaurants.Application.Dishes.Queries.GetDishesForRestaurant
{
    public class GetDishesForRestaurantQueryHandler(IMapper _mapper,
        ILogger<GetDishesForRestaurantQueryHandler> _logger,
        IRestaurantsRepository _restaurantRepository) 
        : IRequestHandler<GetDishesForRestaurantQuery, IEnumerable<DishDto>>
    {
        public async Task<IEnumerable<DishDto>> Handle(GetDishesForRestaurantQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving dishes for retaurant with id: {RestaurantId}...", request.RestaurantId);

            //check if the restaurant of exists
            var restaurant = await _restaurantRepository.GetByIdAsync(request.RestaurantId)
                 ?? throw new NotFoundException(nameof(Restaurants), request.RestaurantId.ToString());

            return _mapper.Map<IEnumerable<DishDto>>(restaurant.Dishes);
        }
    }
}
