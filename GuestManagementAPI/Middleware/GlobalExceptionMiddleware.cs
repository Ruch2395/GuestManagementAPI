using System.Net;
using System.Text.Json;

namespace GuestManagementAPI.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var (statusCode, message) = exception switch
            {
                KeyNotFoundException =>
                    (HttpStatusCode.NotFound, exception.Message),

                InvalidOperationException =>
                    (HttpStatusCode.BadRequest, exception.Message),

                ArgumentException =>
                    (HttpStatusCode.BadRequest, exception.Message),

                _ =>
                    (HttpStatusCode.InternalServerError,
                     "An unexpected error occurred. Please try again later.")
            };

            response.StatusCode = (int)statusCode;

            var errorResponse = new
            {
                status = response.StatusCode,
                error = message,
                timestamp = DateTime.UtcNow
            };

            await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
