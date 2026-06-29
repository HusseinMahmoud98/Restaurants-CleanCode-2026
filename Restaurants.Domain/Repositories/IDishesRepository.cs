using Restaurants.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Domain.Repositories
{
    public interface IDishesRepository
    {
        public Task<IEnumerable<Dish>> GetAllAsync();
        public Task<Dish?> GetByIdAsync(int id);
        public Task<int> CreateAsync(Dish dish);
        //public Task DeleteAsync(Dish dish);
        public Task DeleteAsync(IEnumerable<Dish> dishes);
        public Task UpdateAsync(Dish dish);
    }
}
