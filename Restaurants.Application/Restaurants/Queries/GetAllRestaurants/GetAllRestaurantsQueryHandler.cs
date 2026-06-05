using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Services;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryHandler(IRestaurantsRepository _restaurantsRepository,
        ILogger<GetAllRestaurantsQueryHandler> _logger,
        IMapper _mapper) 
        : IRequestHandler<GetAllRestaurantsQuery, IEnumerable<RestaurantDto>>
    {
        async Task<IEnumerable<RestaurantDto>> IRequestHandler<GetAllRestaurantsQuery, IEnumerable<RestaurantDto>>.Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all restaurants...");

            var restaurants = await _restaurantsRepository.GetAllAsync();
            var restaurantsDto = _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
            return restaurantsDto;
        }
    }
}
