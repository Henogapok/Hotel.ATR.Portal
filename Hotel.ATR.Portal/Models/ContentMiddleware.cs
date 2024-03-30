namespace Hotel.ATR.Portal.Models
{
    public class ContentMiddleware
    {
        private RequestDelegate nextDeletage;
        public ContentMiddleware(RequestDelegate next)
        {
            this.nextDeletage = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if(context.Request.Path.ToString() == "/middleware")
            {
                await context.Response.WriteAsync("это содержание с middleware");
            }
            else
            {
                context.Request.Headers["User-agent"] = "new value";
                await nextDeletage.Invoke(context);
            }
        }
    }

    public static class ContentMiddlewareExtention
    {
        public static IApplicationBuilder UseContentMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ContentMiddleware>();
        }
    }
}
