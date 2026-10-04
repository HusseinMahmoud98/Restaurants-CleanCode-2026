using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandHandler(
        ILogger<CreateRestaurantCommandHandler> _logger, 
        IMapper _mapper, 
        IRestaurantsRepository _restaurantsRepository,
        IUserContext _userContext) 
        : IRequestHandler<CreateRestaurantCommand, int>
    {
        public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            //In Serilog, the @ operator in message templates tells Serilog to serialize the object into structured log properties
            //instead of just calling .ToString().
            //Without @ →
            //Serilog will log the request object using its ToString() method.Usually you’ll just see something like the class name(MyNamespace.RestaurantRequest)
            //unless you override ToString().
            var currentUser = _userContext.GetCurrentUser()
                ?? throw new UnauthenticatedException();

            _logger.LogInformation("{UserEmail} [{UserId}] creating a new restaurant {@Restaurant}",
                currentUser.Email, currentUser.Id, request);

            var restaurant = _mapper.Map<Restaurant>(request);
            restaurant.OwnerId = currentUser.Id;

            await _restaurantsRepository.CreateAsync(restaurant);

            _logger.LogInformation("Restaurant created successfully...");

            return restaurant.Id;
        }
    }
}
