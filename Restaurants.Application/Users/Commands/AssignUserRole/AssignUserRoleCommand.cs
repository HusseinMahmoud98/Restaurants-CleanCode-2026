using MediatR;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Users.Commands.AssignUserRole
{
    public class AssignUserRoleCommand(string Role) : IRequest
    {
    }
}
