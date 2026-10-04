using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Authorization.AuthorizationEvaluators;
using Restaurants.Infrastructure.Authorization.Requirments.CreatedMultipleRestaurantsRequirement;
using Restaurants.Infrastructure.Authorization.Requirments.MinimumAgeRequirement;
using Restaurants.Infrastructure.Authorization.UserClaimsPrincipalFactory;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RestaurantsDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("RestaurantsDb"))
                .EnableSensitiveDataLogging(); //like making the id primary key visible in the console
            });

            services.AddIdentityApiEndpoints<User>()
                .AddRoles<IdentityRole>() //Add Identity Role Claim To User.Claims
                .AddClaimsPrincipalFactory<RestaurantsUserClaimsPrincipalFactory>()
                .AddEntityFrameworkStores<RestaurantsDbContext>(); //Configures Identity to use Entity Framework Core with your RestaurantsDbContext as the backing store.

            services.AddScoped<IRestaurantSeeder, RestaurantSeeder>();
            services.AddScoped<IRestaurantsRepository, RestaurantsRepository>();
            services.AddScoped<IDishesRepository, DishesRepository>();

            services.AddAuthorizationBuilder()
                .AddPolicy(CustomAuthorizationPolicy.HasNationality, builder => builder.RequireClaim(CustomClaimTypes.Nationality, "Egyptian", "American"))
                .AddPolicy(CustomAuthorizationPolicy.AtLeastTwentyYears, builder => builder.AddRequirements(new MinimumAgeRequirement(20)))
                .AddPolicy(CustomAuthorizationPolicy.CreatedAtLeastTwoRestaurants, builder => builder.AddRequirements(new CreatedMultipleRestaurantsRequirement(2)));

            services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, CreatedMultipleRestaurantsRequirementHandler>();
            services.AddScoped<IRestaurantAuthorizationEvaluator, RestaurantAuthorizationEvaluator>();
        }
    }
}
