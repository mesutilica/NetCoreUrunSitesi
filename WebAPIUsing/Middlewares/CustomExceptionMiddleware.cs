namespace WebAPIUsing.Middlewares
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
                string message = "[Request] HTTP Method : " + context.Request.Method + " - Path : " + context.Request.Path + " - Path Value : " + context.Request.Path.Value + " - Controller : " + context.GetRouteValue("Controller") + " - Action : " + context.GetRouteValue("Action") + " - Id : " + context.GetRouteValue("Id");
                Console.WriteLine(message);
                await _next(context);
            }
            catch (Exception ex)
            {
                // burada uygulama genelinde hata yakalanarak loglanabilir
                await context.Response.WriteAsync("Hata Oluştu!");
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
