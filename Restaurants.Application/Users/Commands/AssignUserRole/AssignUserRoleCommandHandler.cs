using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.AssignUserRole
{
    internal class AssignUserRoleCommandHandler(ILogger<AssignUserRoleCommandHandler> _logger,
        IUserContext _userContext, IUserStore<User> _userStore)
        : IRequestHandler<AssignUserRoleCommand>
    {
        public async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
        {
            var user = _userContext.GetCurrentUser();
            _logger.LogInformation("Assigning role {Role} to user {UserId}", request, user!.Id);
            var dbUser = await _userStore.FindByIdAsync(user.Id, cancellationToken);

            if (dbUser is null) throw new NotFoundException(nameof(User),user.Id);

            //dbUser.

        }
    }
}
