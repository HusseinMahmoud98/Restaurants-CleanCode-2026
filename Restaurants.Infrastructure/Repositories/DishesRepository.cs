using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Repositories
{
    internal class DishesRepository(RestaurantsDbContext _dbContext) : IDishesRepository
    {
        public async Task<int> CreateAsync(Dish dish)
        {
            await _dbContext.AddAsync(dish);
            await _dbContext.SaveChangesAsync();
            return dish.Id;
        }

        public async Task DeleteAsync(IEnumerable<Dish> dishes)
        {
            _dbContext.RemoveRange(dishes);
            await _dbContext.SaveChangesAsync();
        }

        //public async Task DeleteAsync(Dish dish)
        //{
        //_dbContext.Remove(dish);
        //await _dbContext.SaveChangesAsync();
        //}

        public async Task<IEnumerable<Dish>> GetAllAsync()
         => await _dbContext.Dishes.ToListAsync();

        public async Task<Dish?> GetByIdAsync(int id)
         => await _dbContext.Dishes.FirstOrDefaultAsync(d => d.Id == id);

        public async Task UpdateAsync(Dish dish)
        {
            _dbContext.Update(dish);
            await _dbContext.SaveChangesAsync();
        }
    }
}
