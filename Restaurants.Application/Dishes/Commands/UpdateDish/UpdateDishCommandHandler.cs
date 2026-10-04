using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.UpdateDish
{
    public class UpdateDishCommandHandler(IMapper _mapper,
        IDishesRepository _dishesRepository,
        ILogger<UpdateDishCommandHandler> _logger)
        : IRequestHandler<UpdateDishCommand>
    {
        public async Task Handle(UpdateDishCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating dish with id: {DishId} with {@UpdatedDish}", request.Id, request);
            var dish = _mapper.Map<Dish>(request);
            await _dishesRepository.UpdateAsync(dish);
        }
    }
}
