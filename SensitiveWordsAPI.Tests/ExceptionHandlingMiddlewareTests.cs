using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SensitiveWordsAPI.Middleware;
using Xunit;

namespace SensitiveWordsAPI.Tests
{
    public class ExceptionHandlingMiddlewareTests
    {
        [Fact]        
        public async Task InvokeAsync_WhenExceptionThrown_Returns500AndErrorMessage()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            RequestDelegate next = (HttpContext _) => throw new Exception("Something went wrong");

            var logger = NullLogger<ExceptionHandlingMiddleware>.Instance;
            var middleware = new ExceptionHandlingMiddleware(next, logger);

            // Act
            await middleware.InvokeAsync(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(context.Response.Body).ReadToEnd();

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
            Assert.Contains("An unexpected error occurred", responseBody);
        }

        [Fact]
        public async Task InvokeAsync_WhenNoExceptionThrown_CallsNextMiddleware()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var wasCalled = false;

            RequestDelegate next = (HttpContext _) =>
            {
                wasCalled = true;
                return Task.CompletedTask;
            };

            var logger = NullLogger<ExceptionHandlingMiddleware>.Instance;
            var middleware = new ExceptionHandlingMiddleware(next, logger);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.True(wasCalled);
        }
    }
}