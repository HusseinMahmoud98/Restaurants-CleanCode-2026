using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Dishes.Commands.DeleteDish
{
    public class DeleteDishCommandHandler(ILogger<DeleteDishCommandHandler> _logger,
        IDishesRepository _dishesRepository,
        IRestaurantsRepository _restaurantsRepository)
        : IRequestHandler<DeleteDishesCommand>
    {
        public async Task Handle(DeleteDishesCommand request, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Deleting dishes For Restaurant id: {RestaurantId}", request.RestaurantId);

            var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId)
                 ?? throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

            //Note: without .ToList() the code will crash in the second iteration as we are modifiing the original list and thats forbiden in foreach
            //foreach (var dish in restaurant.Dishes.ToList())
            //{
            //await _dishesRepository.DeleteAsync(dish);
            //_logger.LogInformation("Dish with id: {DishId} For restaurnat id {RestauranId} deleted", dish.Id, request.RestaurantId);
            //}

            await _dishesRepository.DeleteAsync(restaurant.Dishes);

        }
    }
}

