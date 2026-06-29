using AutoMapper;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.UpdateDish;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dishes.Profiles
{
    public class DishesProfile : Profile
    {
        public DishesProfile() 
        {
            CreateMap<Dish, DishDto>();

            CreateMap<CreateDishCommand, Dish>();
            CreateMap<UpdateDishCommand, Dish>();
        }

    }
}
