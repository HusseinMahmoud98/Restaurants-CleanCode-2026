using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands
{
    public class UpdateUserDetailsCommandHandler(ILogger<UpdateUserDetailsCommand> _logger,
        IUserContext userContext, IUserStore<User> _userStore)
        : IRequestHandler<UpdateUserDetailsCommand>
    {
        public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
        {
            var user = userContext.GetCurrentUser();
            _logger.LogInformation("Updating user: {userId}, with {@Request}", user!.Id, request);
            var dbuser = await _userStore.FindByIdAsync(user!.Id, cancellationToken);

            if (dbuser is null) throw new NotFoundException(nameof(User), user!.Id);

            dbuser.Nationality = request.Nationality;
            dbuser.DateOfBirth = request.DateOfBirth;

            await _userStore.UpdateAsync(dbuser, cancellationToken);
        }
    }
}
