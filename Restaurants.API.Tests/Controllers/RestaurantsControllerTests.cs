using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.API.Tests;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Restaurants.API.Controllers.Tests
{
    public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        //this factory object, will allow us to create http client that will send http request
        //aganist api-inMemory
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();
        public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                //override the services that are registered by the api
                //to bypass authentication & authorization
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.Replace(ServiceDescriptor.Scoped(typeof(IRestaurantsRepository), _ => _restaurantsRepositoryMock.Object));
                });
            });
        }

        [Fact()]
        public async Task GetAll_ForValidRequest_Returns200Ok()
        {
            // arrange
            var client = _factory.CreateClient();


            // act
            var response = await client.GetAsync("/api/restaurants?pageNumber=1&pageSize=10");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact()]
        public async Task GetAll_ForInvalidRequest_Returns400BadRequest()
        {
            // arrange
            var client = _factory.CreateClient();


            // act
            var response = await client.GetAsync("/api/restaurants");

            // assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact()]
        public async Task GetById_ForNonExistingId_ShouldReturn404NotFound()
        {
            // arrange
            var id = 123;

            _restaurantsRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync((Restaurant?)null);

            var client = _factory.CreateClient();

            // act
            var response = await client.GetAsync($"/api/restaurants/{id}");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact()]
        public async Task GetById_ForExistingId_ShouldReturn200Ok()
        {
            // arrange
            var id = 100;

            var restaurant = new Restaurant
            {
                Id = id,
                Name = "Test Restaurant",
                Description = "Test Description",
            };

            _restaurantsRepositoryMock.Setup(repo => repo.GetByIdAsync(id)).ReturnsAsync(restaurant);

            var client = _factory.CreateClient();

            // act
            var response = await client.GetAsync($"/api/restaurants/{id}");
            var responseContent = await response.Content.ReadFromJsonAsync<RestaurantDto>();



            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseContent.Should().NotBeNull();
            responseContent.Name.Should().Be(restaurant.Name);
            responseContent.Description.Should().Be(restaurant.Description);

        }
    }
}