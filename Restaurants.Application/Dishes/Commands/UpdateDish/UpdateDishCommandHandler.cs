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
            var dish = _mapper.Map<Dish>(request);
            await _dishesRepository.UpdateAsync(dish);
        }
    }
}
