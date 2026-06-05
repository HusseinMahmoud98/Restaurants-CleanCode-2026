using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Repositories
{
    internal class RestaurantsRepository(RestaurantsDbContext _dbContext) : IRestaurantsRepository
    {
        //Add restaurant to the database
        public async Task<int> CreateAsync(Restaurant restaurant)
        { 
            await _dbContext.AddAsync(restaurant);
            await _dbContext.SaveChangesAsync();
            return restaurant.Id;
        }
        
        //Get all the restaurants from the database
        public async Task<IEnumerable<Restaurant>> GetAllAsync()
            => await _dbContext.Restaurants.Include(r => r.Dishes).ToListAsync();

        //Get Restaurant with specific id from the database
        public async Task<Restaurant?> GetByIdAsync(int id)
            => await _dbContext.Restaurants.Include(r => r.Dishes).FirstOrDefaultAsync(r => r.Id == id);

        //Remove restaurant with given id from the database
        public async Task DeleteAsync(Restaurant restaurant)
        {
            _dbContext.Remove(restaurant);
            await _dbContext.SaveChangesAsync();
        }

        //Update restaurant with given id from the database
        public async Task UpdateAsync(Restaurant restaurant)
        {
            _dbContext.Update(restaurant);
            await _dbContext.SaveChangesAsync();
        }
    }
}
