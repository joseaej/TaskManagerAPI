namespace TasksManagerAPI.Controllers
{
    public class ErrorHandlingController
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingController> _logger;

        public ErrorHandlingController(RequestDelegate next, ILogger<ErrorHandlingController> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode == 401)
                {
                    await HandleCustomError(context, 401, "You are not authorised to access this resource.");
                }
                else if (context.Response.StatusCode == 404)
                {
                    await HandleCustomError(context, 404, "The requested resource was not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado");
                await HandleCustomError(context, 500, "An internal server error occurred.");
            }
        }

        private async Task HandleCustomError(HttpContext context, int statusCode, string message)
        {
            try
            {

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = statusCode;

                var response = new
                {
                    status = statusCode,
                    error = message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado");
            }

        }
    }

}
