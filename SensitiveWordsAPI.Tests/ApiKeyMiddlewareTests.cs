using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using SensitiveWordsAPI.Middleware;


namespace SensitiveWordsAPI.Tests
{
    public class ApiKeyMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_ExternalRouteWithValidApiKey_CallsNext()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Path = "/api/external/Sanitize";
            context.Request.Headers["X-Api-Key"] = "test-key";

            var wasNextCalled = false;
            RequestDelegate next = (HttpContext hc) =>
            {
                wasNextCalled = true;
                return Task.CompletedTask;
            };

            var config = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    { "ApiKeySettings:InternalApiKey", "test-key" }
                })
            .Build();

            var middleware = new ApiKeyMiddleware(next, config);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.True(wasNextCalled);
        }

        //[Fact]        
        //public async Task InvokeAsync_ExternalRouteWithInvalidApiKey_Returns401()
        //{
        //    var context = new DefaultHttpContext();
        //    context.Request.Path = "/api/external/Sanitize";
        //    context.Request.Headers["X-Api-Key"] = "wrong-key";
        //    context.Response.Body = new MemoryStream();

        //    var wasNextCalled = false;
        //    RequestDelegate next = (HttpContext hc) =>
        //    {
        //        wasNextCalled = true;
        //        return Task.CompletedTask;  // Simulate passing to the next middleware
        //    };

        //    var config = new ConfigurationBuilder()
        //        .AddInMemoryCollection(
        //            new Dictionary<string, string?>
        //            {
        //        { "ApiKeySettings:InternalApiKey", "correct-key" }
        //            })
        //        .Build();

        //    var middleware = new ApiKeyMiddleware(next, config);

        //    // Act
        //    await middleware.InvokeAsync(context);
        //    context.Response.Body.Seek(0, SeekOrigin.Begin);
        //    var reader = new StreamReader(context.Response.Body);
        //    var bodyText = await reader.ReadToEndAsync();

        //    // Assert
        //    Assert.Equal(401, context.Response.StatusCode); // Expect 401 Unauthorized
        //    Assert.Contains("Unauthorized", bodyText); // Check if the response body contains "Unauthorized"
        //}

        [Fact]        
        public async Task InvokeAsync_NonInternalRoute_SkipsApiKeyCheck()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Request.Path = "/api/external/Sanitize"; // external route

            var wasNextCalled = false;
            RequestDelegate next = (HttpContext hc) =>
            {
                wasNextCalled = true;
                return Task.CompletedTask;
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
            { "ApiKeySettings:InternalApiKey", "any-key" }
                })
                .Build();

            var middleware = new ApiKeyMiddleware(next, config);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.True(wasNextCalled);
        }
    }
}
