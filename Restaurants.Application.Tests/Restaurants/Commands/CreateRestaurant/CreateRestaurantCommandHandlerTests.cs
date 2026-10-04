using Xunit;
using Moq;
using AutoMapper;
using Restaurants.Domain.Repositories;
using Restaurants.Domain.Entities;
using Restaurants.Application.Users;
using Microsoft.Extensions.Logging;
using FluentAssertions;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests
{
    public class CreateRestaurantCommandHandlerTests
    {
        [Fact()]
        public async Task Handle_ForValidCommand_ReturnsCreatedRestaurantIdAsync()
        {
            // arrange
            // mocks definition
            var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();
            var mapperMock = new Mock<IMapper>();
            var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();

            // command and restaurant definition
            var command = new CreateRestaurantCommand();
            var restaurant = new Restaurant();

            // mapper setup
            mapperMock.Setup(m => m.Map<Restaurant>(command))
                .Returns(restaurant);

            // restaurant repository mocking
            restaurantRepositoryMock
                //.Setup(repo => repo.CreateAsync(restaurant))
                .Setup(repo => repo.CreateAsync(It.IsAny<Restaurant>()))
                .Callback<Restaurant>(r => r.Id = 1) // mimic EF Core assigning Id
                .ReturnsAsync(1);

            var userContextMock = new Mock<IUserContext>();
            var currentUser = new CurrentUser("owner-id", "test@test.com", [], null, null);

            userContextMock.Setup(u => u.GetCurrentUser()).Returns(currentUser);

            var commandHandler = new CreateRestaurantCommandHandler(
                loggerMock.Object,
                mapperMock.Object,
                restaurantRepositoryMock.Object,
                userContextMock.Object);

            // act
            var result = await commandHandler.Handle(command, CancellationToken.None);

            // assert
            result.Should().Be(1);
            restaurant.OwnerId.Should().Be("owner-id");
            restaurantRepositoryMock.Verify(r => r.CreateAsync(restaurant), Times.Once);
        }
    }
}