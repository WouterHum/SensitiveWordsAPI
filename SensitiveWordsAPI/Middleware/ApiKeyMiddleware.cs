using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace SensitiveWordsAPI.Middleware
{
    /// <summary>
    /// Middleware to enforce API key authentication for internal endpoints.
    /// </summary>
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEY_HEADER_NAME = "X-Api-Key";
        private readonly string? _apiKey;

        /// <summary>
        /// Initializes a new instance of the ApiKeyMiddleware class with the specified configuration.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="configuration"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _apiKey = configuration["ApiKeySettings:InternalApiKey"]
                      ?? throw new InvalidOperationException("API Key is missing from configuration.");
        }

        /// <summary>
        /// Invokes the middleware to check for API key authentication.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // Only enforce API key on /internal endpoints
            if (context.Request.Path.StartsWithSegments("/api/internal", StringComparison.OrdinalIgnoreCase))
            {
                var apiKey = context.Request.Headers["X-Api-Key"].ToString();

                // Check if the API key is present in the request headers
                if (string.IsNullOrEmpty(apiKey) || !apiKey.Equals(_apiKey))
                {
                    // Return 401 if the API key is missing or invalid
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }

                // Check if the API key is present in the request headers
                if (!context.Request.Headers.TryGetValue("X-Api-Key", out var extractedApiKey))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("API Key was not provided.");
                    return;
                }

                // Check if the API key is valid
                if (!string.Equals(_apiKey, extractedApiKey, StringComparison.Ordinal))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Unauthorized client.");
                    return;
                }
            }

            await _next(context);
        }
    }
}
