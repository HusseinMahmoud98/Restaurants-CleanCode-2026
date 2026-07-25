using FluentValidation;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
    {
        private int[] allowedPageSizes = [5, 10, 15, 30];

        public GetAllRestaurantsQueryValidator()
        {
            RuleFor(r => r.PageSize)
                .Must(pageSize => allowedPageSizes.Contains(pageSize))
                .WithMessage($"Page size must be one of the following values: [{string.Join(", ", allowedPageSizes)}]");
            
            RuleFor(r => r.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page number must be greater than or equals 1.");
        }
    }
}
