using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.DeleteDish;
using Restaurants.Application.Dishes.Commands.UpdateDish;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Queries.GetDishById;
using Restaurants.Application.Dishes.Queries.GetDishesForRestaurant;

namespace Restaurants.API.Controllers
{  
    [ApiController]
    [Route("api/restaurants/{restaurantId}/dishes")]
    public class DishesController(IMediator mediator) : ControllerBase
    {
       [HttpPost]
       public async Task<IActionResult> CreateDish([FromRoute] int restaurantId,[FromBody] CreateDishCommand command)
       {
           command.RestaurantId = restaurantId;
           var dishId = await mediator.Send(command);
           return CreatedAtAction(nameof(GetByIdForRestaurant), new { restaurantId, dishId }, null);
       }

       [HttpGet("{dishId}")]
       public async Task<ActionResult<DishDto>> GetByIdForRestaurant([FromRoute] int restaurantId,[FromRoute] int dishId)
       {
            var dishDto = await mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId, dishId));
            return Ok(dishDto);
       }

       [HttpGet]
       public async Task<ActionResult<IEnumerable<GetDishesForRestaurantQuery>>> GetAllForRestaurant([FromRoute] int restaurantId)
       { 
            var dishesDto = await mediator.Send(new GetDishesForRestaurantQuery(restaurantId)); 
            return Ok(dishesDto);
       }

       [HttpPut("{id}")]
       public async Task<IActionResult> Update([FromRoute] int restaurantId, [FromBody] UpdateDishCommand command, int id)
       { 
            command.Id = id;
            await mediator.Send(command);
            return NoContent();
       }

       [HttpDelete]
       public async Task<IActionResult> DeletedDishesForRestaurant(int restaurantId) 
       {
           await mediator.Send(new DeleteDishesCommand(restaurantId));
           return NoContent();
       }
    }
}