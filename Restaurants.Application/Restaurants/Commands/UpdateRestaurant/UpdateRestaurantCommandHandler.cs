using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommandHandler(ILogger<UpdateRestaurantCommandHandler> _logger,
        IRestaurantsRepository _restaurantsRepository,
        IMapper _mapper)
        : IRequestHandler<UpdateRestaurantCommand>
    {
        public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            //In Serilog, the @ operator in message templates tells Serilog to serialize the object into structured log properties
            //instead of just calling .ToString().
            //Without @ →
            //Serilog will log the request object using its ToString() method.Usually you’ll just see something like the class name(MyNamespace.RestaurantRequest)
            //unless you override ToString().
            _logger.LogInformation("Updating restaurant with id: {RestaurantId} with {@UpdatedRestaurant}", request.Id, request);

            var restaurant = await _restaurantsRepository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());

            _mapper.Map(request, restaurant);

            await _restaurantsRepository.UpdateAsync(restaurant);
            _logger.LogInformation("Restaurant with id {RestaurantId} updated", request.Id);
        }
    }
}
