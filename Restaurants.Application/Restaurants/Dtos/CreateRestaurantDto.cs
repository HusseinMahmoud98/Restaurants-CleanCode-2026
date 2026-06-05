using Restaurants.Application.Dishes.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Application.Restaurants.Dtos
{
    public class CreateRestaurantDto
    {
        [StringLength( 100, MinimumLength = 3)]
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        [Required(ErrorMessage = "Please provide a valid category")]
        public string Category { get; set; } = default!;
        public bool HasDelivery { get; set; }
        [EmailAddress]
        public string? ContactEmail { get; set; }
        [Phone(ErrorMessage = "Please provide a valid contact number")]
        public string? ContactNumber { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Please provide a valid postal code (XX-XXX).")]
        public string? PostalCode { get; set; }
    }
}
