using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Repositories;


namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryHandler(IRestaurantsRepository _restaurantsRepository,
        ILogger<GetAllRestaurantsQueryHandler> _logger,
        IMapper _mapper) 
        : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDto>>
    {
        async Task<PagedResult<RestaurantDto>> IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDto>>.Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all restaurants...");

            var (restaurants, totalCount) = await _restaurantsRepository.GetAllAsync(request.SearchPhrase, request.PageSize, request.PageNumber, request.SortBy, request.SortingDirection);
            var restaurantsDto = _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

            return new PagedResult<RestaurantDto>(restaurantsDto, totalCount, request.PageSize, request.PageNumber);
        }
    }
}
