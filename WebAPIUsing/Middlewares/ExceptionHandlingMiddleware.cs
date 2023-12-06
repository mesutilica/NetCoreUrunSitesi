using Microsoft.AspNetCore.Mvc;

namespace WebAPIUsing.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                /*if (!string.IsNullOrEmpty(context.Session.GetString("token")))
                {
                    context.Request.Headers.Append("Bearer", context.Session.GetString("token")); //context.Response.Redirect("/admin/logout?msg=AccessDenied");
                    context.Response.Headers.Add("Authorization", "Bearer " + context.Session.GetString("token"));
                    context.Request.Headers.Add("Authorization", "Basic Pusat");
                    context.Request.Headers["Authorization"] = "Bearer " + context.Session.GetString("token");
                    context.Response.Headers["Authorization"] = "Bearer " + context.Session.GetString("token");
                    await context.Response.WriteAsync(context.Session.GetString("token"));
                }*/
                string message = "[Request] HTTP Method : " + context.Request.Method + " - Path : " + context.Request.Path + " - Path Value : " + context.Request.Path.Value + " - Controller : " + context.GetRouteValue("Controller") + " - Action : " + context.GetRouteValue("Action") + " - Id : " + context.GetRouteValue("Id");
                Console.WriteLine(message);
                await _next(context);
            }
            catch (Exception exception)
            {
                // burada uygulama genelinde hata yakalanarak loglanabilir
                _logger.LogError(exception, "Hata Oluştu: {Message}", exception.Message);
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server Error"
                };
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(problemDetails);
                //await context.Response.WriteAsync("Hata Oluştu!");
            }
        }
    }
}
