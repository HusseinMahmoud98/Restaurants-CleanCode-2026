using AutoMapper;
using Castle.Core.Logging;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using System.Runtime.CompilerServices;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Tests
{
    public class UpdateRestaurantCommandHandlerTests
    {
        private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IRestaurantsRepository> _restaurantRepositoryMock;
        private readonly Mock<IRestaurantAuthorizationEvaluator> _restaurantAuthorizationServiceMock;

        private readonly UpdateRestaurantCommandHandler _commandHandler;


        public UpdateRestaurantCommandHandlerTests()
        {
            _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
            _mapperMock = new Mock<IMapper>();
            _restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
            _restaurantAuthorizationServiceMock = new Mock<IRestaurantAuthorizationEvaluator>();

            _commandHandler = new UpdateRestaurantCommandHandler(
                _loggerMock.Object,
                _restaurantRepositoryMock.Object,
                _restaurantAuthorizationServiceMock.Object,
                _mapperMock.Object);
        }

        [Fact()]
        public async Task Handle_ForValidCommand_ShouldUpdateRestaurantAsync()
        {
            // arrange
            // command and restaurant definition
            var restaurantId = 1;
            var command = new UpdateRestaurantCommand()
            {
                Id = restaurantId,
                Name = "Updated Restaurant Test",
                Description = "Updated Category Test",
                HasDelivery = true
            };

            var restaurant = new Restaurant()
            {
                Id = restaurantId,
                Name = "Restaurant Test",
                Description = "Category Test"
            };

            // mapper setup
            _mapperMock.Setup(m => m.Map<Restaurant>(command))
                .Returns(restaurant);

            // restaurant repository mocking
            _restaurantRepositoryMock.Setup(repo => repo.GetByIdAsync(restaurantId))
                .ReturnsAsync(restaurant);

            _restaurantRepositoryMock.Setup(repo => repo.UpdateAsync(restaurant));

            // restaurant authorization service mocking
            _restaurantAuthorizationServiceMock.Setup(auth => auth.Authorize(restaurant, ResourceOperation.Update))
                .Returns(true);

            // act
            //Action action = async () =>
            await _commandHandler.Handle(command, CancellationToken.None);


            // assert
            _restaurantRepositoryMock.Verify(repo => repo.UpdateAsync(restaurant), Times.Once);
            _mapperMock.Verify(m => m.Map(command, restaurant), Times.Once);
            _restaurantAuthorizationServiceMock.Verify(auth => auth.Authorize(restaurant, ResourceOperation.Update), Times.Once);
        }

        [Fact()]
        public async Task Handle_ForNonExistingRestaurant_ShouldThrowNotFoundExceptionAsync()
        {
            // arrange
            var restaurantId = 1;
            var command = new UpdateRestaurantCommand()
            {
                Id = restaurantId,
                Name = "Updated Restaurant Test",
                Description = "Updated Category Test",
                HasDelivery = true
            };
            _restaurantRepositoryMock.Setup(repo => repo.GetByIdAsync(restaurantId))
                .ReturnsAsync((Restaurant?)null);
            // act
            Func<Task> act = async () => await _commandHandler.Handle(command, CancellationToken.None);
            
            // assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact()]
        public async Task Handle_ForUnauthorizedUser_ShouldThrowForbidExceptionAsync()
        {
            // arrange
            var restaurantId = 1;
            var command = new UpdateRestaurantCommand()
            {
                Id = restaurantId,
                Name = "Updated Restaurant Test",
                Description = "Updated Category Test",
                HasDelivery = true
            };

            var restaurant = new Restaurant()
            {
                Id = restaurantId
            };

            _restaurantRepositoryMock.Setup(repo => repo.GetByIdAsync(restaurantId))
                .ReturnsAsync(restaurant);

            _restaurantAuthorizationServiceMock.Setup(auth => auth.Authorize(restaurant, ResourceOperation.Update))
                .Returns(false);

           // act
           Func<Task> act = async () => await _commandHandler.Handle(command, CancellationToken.None);
            
            // assert
            await act.Should().ThrowAsync<ForbidException>();
        }
    }
}