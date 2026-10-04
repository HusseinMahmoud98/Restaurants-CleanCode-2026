using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Domain.Constants;
using System.Security.Claims;
using Xunit;

namespace Restaurants.Application.Users.Tests
{
    public class UserContextTests
    {
        [Fact()]
        public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
        {
            // arrange
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var dateOfBirth = new DateOnly(1990, 1, 1);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Email, "test@test.com"),
                new Claim(ClaimTypes.Role, UserRoles.Admin),
                new Claim(ClaimTypes.Role, UserRoles.User),
                new Claim(CustomClaimTypes.Nationality, "Egyptian"),
                new Claim(CustomClaimTypes.DateOfBirth, dateOfBirth.ToString("yyyy-MM-dd"))
            };

            var userClaimsPrincipals = new ClaimsPrincipal(new ClaimsIdentity(claims, "AuthenticationType - Like Bearer"));

            httpContextAccessorMock.Setup(m => m.HttpContext).Returns(new DefaultHttpContext
            {
                User = userClaimsPrincipals
            });

            var userContext = new UserContext(httpContextAccessorMock.Object);

            // act
            var currentUser = userContext.GetCurrentUser();


            // assert
            currentUser.Should().NotBeNull();
            currentUser!.Id.Should().Be("1");
            currentUser.Email.Should().Be("test@test.com");
            currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User);
            currentUser.Nationality.Should().Be("Egyptian");
            currentUser.DateOfBirth.Should().Be(dateOfBirth);

        }

        [Fact()]
        public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
        {
            // arrange
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(m => m.HttpContext).Returns((HttpContext)null);

            var userContext = new UserContext(httpContextAccessorMock.Object);

            // act
            // Here we used action because we expect that and exception is thrown and we wanna verify that
            Action action = () => userContext.GetCurrentUser();

            // assert 
            action.Should().Throw<InvalidOperationException>()
                .WithMessage("User context is not present");
        }
    
    }
}