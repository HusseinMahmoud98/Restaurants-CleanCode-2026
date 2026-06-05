using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQueryHandler (IRestaurantsRepository _restaurantsRepository,
        ILogger<GetRestaurantByIdQueryHandler> _logger,
        IMapper _mapper)
        : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto>
    {
        public async Task<RestaurantDto> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
        {
            var resaurant = await _restaurantsRepository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

            _logger.LogInformation("Getting the restaurant with id: {RestaurantId}...", request.Id);

            var resaurantDto = _mapper.Map<RestaurantDto>(resaurant);
            return resaurantDto;
        }
    }
}
