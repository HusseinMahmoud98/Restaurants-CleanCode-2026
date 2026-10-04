using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Infrastructure.Authorization.Requirments.CreatedMultipleRestaurantsRequirement.Tests
{
    public class CreatedMultipleRestaurantsRequirementHandlerTests
    {
        [Fact()]
        public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
        {
            // arrange
            var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

            var restaurants = new List<Restaurant>()
            {
                new()
                {
                    OwnerId = currentUser.Id
                },

                new()
                {
                    OwnerId = currentUser.Id
                },

                new()
                {
                    OwnerId = "2",
                }
            };

            var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
            restaurantRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(restaurants);


            var requirement = new CreatedMultipleRestaurantsRequirement(2);

            var handler = new CreatedMultipleRestaurantsRequirementHandler(
                Mock.Of<ILogger<CreatedMultipleRestaurantsRequirementHandler>>(), 
                userContextMock.Object,
                restaurantRepositoryMock.Object);

            var authorizationHandlerContext = new AuthorizationHandlerContext([requirement], null, null);

            // act
            await handler.HandleAsync(authorizationHandlerContext);

            // assert
            authorizationHandlerContext.HasSucceeded.Should().BeTrue();

        }

        [Fact()]
        public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_ShouldFail()
        {
            // arrange

            var currentUser = new CurrentUser("1", "test@test.com", [], null, null);

            var userContextMock = new Mock<IUserContext>();
            userContextMock.Setup(m => m.GetCurrentUser()).Returns(currentUser);

            var restaurants = new List<Restaurant>()
            {
                new()
                {
                    OwnerId = currentUser.Id
                },


                new()
                {
                    OwnerId = "2",
                }
            };

            var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
            restaurantRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(restaurants);


            var requiremnt = new CreatedMultipleRestaurantsRequirement(2);

            var handler = new CreatedMultipleRestaurantsRequirementHandler(
                Mock.Of<ILogger<CreatedMultipleRestaurantsRequirementHandler>>(),
                userContextMock.Object,
                restaurantRepositoryMock.Object);

            var authorizationHandlerContext = new AuthorizationHandlerContext([requiremnt], null, null);

            // act
            await handler.HandleAsync(authorizationHandlerContext);

            // assert
            authorizationHandlerContext.HasSucceeded.Should().BeFalse();
            authorizationHandlerContext.HasFailed.Should().BeTrue();

        }
    }
}