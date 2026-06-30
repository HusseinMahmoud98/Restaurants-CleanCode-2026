using MediatR;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Users.Commands
{
    public class UpdateUserDetailsCommand : IRequest
    {
        public DateOnly? DateOfBirth { get; set; }
        public string? Nationality { get; set; }
    }
}
