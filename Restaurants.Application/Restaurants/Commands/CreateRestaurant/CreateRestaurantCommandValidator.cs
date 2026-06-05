using FluentValidation;
using Restaurants.Application.Restaurants.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
    {
        private readonly List<string> validCategories = ["Italian", "Mexican", "American", "Japanese", "Indian"];


        public CreateRestaurantCommandValidator()
        {
            RuleFor(request => request.Name)
                .Length(3, 100);

            //RuleFor(dto => dto.Description)
            //    .NotEmpty().WithMessage("Description is required.");


            //RuleFor(dto => dto.Category)
            //    .NotEmpty()
            //    .WithMessage("Category is required.");

            RuleFor(request => request.Category)
                .Must(validCategories.Contains)
                .When(request => !string.IsNullOrWhiteSpace(request.Category))
                .WithMessage($"Invalid category, please choose from the valid categories:\n{string.Join(", ", validCategories)}.");

            //.Custom((value, context) =>
            //{
            //    var isValidCategory = validCategories.Contains(value);

            //    if (!isValidCategory)
            //    {
            //        context.AddFailure("Category", "Invalid category, please choose from the valid categories.");
            //    }
            //});


            //RuleFor(dto => dto.ContactEmail)
            //    .EmailAddress()
            //    .WithMessage("Please insert a valid email address");


            RuleFor(request => request.PostalCode)
                .Matches(@"^\d{2}-\d{3}$")
                .WithMessage("Please provide a valid postal code (XX-XXX).");
        }
    }
}
