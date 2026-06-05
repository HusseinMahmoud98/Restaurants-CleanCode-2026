using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Services
{
    internal class RestaurantsService(IRestaurantsRepository restaurantsRepository,
        ILogger<RestaurantsService> logger,
        IMapper mapper) : IRestaurantsService
    {
        public async Task<int> CreateRestaurantAsync(CreateRestaurantDto createRestaurantDto)
        {
            var restaurant = mapper.Map<Restaurant>(createRestaurantDto);

            await restaurantsRepository.CreateAsync(restaurant);

            logger.LogInformation("restaurant created successfully...");

            return restaurant.Id;
        }

        public async Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync()
        {
            logger.LogInformation("Getting all restaurants...");

            var restaurants = await restaurantsRepository.GetAllAsync();
            var restaurantsDto = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
            return restaurantsDto;
        }

        public async Task<RestaurantDto?> GetRestaurantByIdAsync(int id)
        {
            logger.LogInformation($"Getting the restaurant with id: {id}...");
            
            var resaurant = await restaurantsRepository.GetByIdAsync(id);
            var resaurantDto = mapper.Map<RestaurantDto>(resaurant);
            return resaurantDto;
        }
    }


}
