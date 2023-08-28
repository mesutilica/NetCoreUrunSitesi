using System.Net;
using System.Text.Json;

namespace WebAPI.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                string message = "[Request] HTTP Method : " + context.Request.Method + " - Path : " + context.Request.Path + " - Path Value : " + context.Request.Path.Value;
                Console.WriteLine(message);
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var result = JsonSerializer.Serialize(new { error = ex.Message });
                await context.Response.WriteAsync(result);
            }
        }
    }
    public static class CutomExceptionMiddlewareExtension
    {
        public static IApplicationBuilder UseCutomExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}
