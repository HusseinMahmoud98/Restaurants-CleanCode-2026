using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Services
{
    public interface IRestaurantsService
    {
        public Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync();
        public Task<RestaurantDto?> GetRestaurantByIdAsync(int id);
        public Task<int> CreateRestaurantAsync(CreateRestaurantDto createRestaurantDto);
    }
}