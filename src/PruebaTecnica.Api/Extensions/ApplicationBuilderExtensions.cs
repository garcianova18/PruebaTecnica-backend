using PruebaTecnica.Api.Middleware;

namespace PruebaTecnica.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGlobalException(this IApplicationBuilder app)
        {
           return app.UseMiddleware<GlobalExceptionHandler>();
        }
    }
}
