using ItemStore.WebApi.Exceptions;
using System.Net;
using System.Text.Json;

namespace ItemStore.WebApi.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case ItemNotFoundException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case ItemListEmptyException e:
                        response.StatusCode = (int)HttpStatusCode.NoContent;
                        break;
                    case InvalidItemException e:
                        response.StatusCode = (int)HttpStatusCode.Conflict;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        _logger.LogError(error, error.Message);
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
