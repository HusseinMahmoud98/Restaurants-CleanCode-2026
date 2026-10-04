using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Domain.Exceptions;
using Xunit;

namespace Restaurants.API.Middlewares.Tests
{
    public class ErrorHandlingMiddlewareTests
    {
        [Fact()]
        public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
        {
            // arrange
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var nextDelegateMock = new Mock<RequestDelegate>();

            var errorHandlingMiddleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var httpContext = new DefaultHttpContext();

            // act
            //await errorHandlingMiddleware.InvokeAsync(httpContext, (innerHttpContext) => Task.CompletedTask);
            await errorHandlingMiddleware.InvokeAsync(httpContext, nextDelegateMock.Object);



            // assert
            // No exception should be thrown, so the test will pass if it reaches this point without errors.
            //nextDelegateMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Once, "The next delegate should be called exactly once.");
            nextDelegateMock.Verify(next => next(httpContext), Times.Once, "The next delegate should be called exactly once.");
        }

        [Fact()]
        public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCodeTo404()
        {
            // arrange
            var httpContext = new DefaultHttpContext();
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var errorHandlingMiddleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var notFoundException = new NotFoundException(nameof(Restaurants), "1");

            // act
            await errorHandlingMiddleware.InvokeAsync(httpContext, _ => throw notFoundException);

            // assert
            httpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact()]
        public async Task InvokeAsync_WhenForbidenExceptionThrown_ShouldSetStatusCodeTo403()
        {
            // arrange
            var httpContext = new DefaultHttpContext();
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var errorHandlingMiddleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var forbidenException = new ForbidException();

            // act
            await errorHandlingMiddleware.InvokeAsync(httpContext, _ => throw forbidenException);

            // assert
            httpContext.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
        }

        [Fact()]
        public async Task InvokeAsync_WhenUnauthenticatedExceptionThrown_ShouldSetStatusCodeTo401()
        {
            // arrange
            var httpContext = new DefaultHttpContext();
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var errorHandlingMiddleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var unauthenticatedException = new UnauthenticatedException();

            // act
            await errorHandlingMiddleware.InvokeAsync(httpContext, _ => throw unauthenticatedException);

            // assert
            httpContext.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact()]
        public async Task InvokeAsync_WhenGenericExceptionThrown_ShouldSetStatusCodeTo500()
        {
            // arrange
            var httpContext = new DefaultHttpContext();
            var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            var errorHandlingMiddleware = new ErrorHandlingMiddleware(loggerMock.Object);
            var genericException = new Exception();
            
            // act
            await errorHandlingMiddleware.InvokeAsync(httpContext, _ => throw genericException);

            // assert
            httpContext.Response.StatusCode.Should()
                .Be(StatusCodes.Status500InternalServerError);
        }
    }
}