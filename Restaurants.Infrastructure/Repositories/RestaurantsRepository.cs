using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using System.Linq.Expressions;

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

        public async Task<(IEnumerable<Restaurant>, int)> GetAllAsync(string? searchPhrase, int pageSize,int pageNumber, string? sortBy, SortingDirection sortingDirection)
        {
            searchPhrase = searchPhrase?.ToLower();

            var baseQuery = _dbContext.Restaurants
                .Where(r => (string.IsNullOrWhiteSpace(searchPhrase)) || r.Name.ToLower().Contains(searchPhrase) || r.Description.ToLower().Contains(searchPhrase!));
                
            var totalCount = await baseQuery.CountAsync();

            if (sortBy != null)
            {
                var columnSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    {nameof(Restaurant.Name), r => r.Name},
                    {nameof(Restaurant.Description), r => r.Description},
                    {nameof(Restaurant.Category), r => r.Category}
                };

                var selectedColumn = columnSelector[sortBy];

                baseQuery = sortingDirection == SortingDirection.Ascending
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var restaurant = await baseQuery.Include(r => r.Dishes)
                .Where(r => (string.IsNullOrWhiteSpace(searchPhrase)) || r.Name.ToLower().Contains(searchPhrase) || r.Description.ToLower().Contains(searchPhrase!))
                .Skip(pageSize*(pageNumber-1))
                .Take(pageSize)
                .ToListAsync();

            return (restaurant, totalCount);
        }
    }
}
