using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
    {
        private int[] allowedPageSizes = [5, 10, 15, 30];
        private string[] allowedSortByColumnName = [nameof(RestaurantDto.Name), 
            nameof(RestaurantDto.Category),
            nameof(RestaurantDto.Description)];

        public GetAllRestaurantsQueryValidator()
        {
            RuleFor(r => r.PageSize)
                .Must(pageSize => allowedPageSizes.Contains(pageSize))
                .WithMessage($"Page size must be one of the following values: [{string.Join(", ", allowedPageSizes)}]");
            
            RuleFor(r => r.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page number must be greater than or equals 1.");

            RuleFor(r => r.SortBy)
                .Must(sortBy => string.IsNullOrWhiteSpace(sortBy) || allowedSortByColumnName.Contains(sortBy))
                .WithMessage($"Sort by is optional or must be one of the following values: [{string.Join(", ", allowedSortByColumnName)}]");
        }
    }
}
