using Lab4.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Lab4.Middleware
{
    public static class DbInitializerMiddleware
    {
        public static IApplicationBuilder UseDbInitializer(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AdvertisingDbContext>();
                    DbInitializer.Initialize(db);
                }

                await next.Invoke();
            });

            return app;
        }
    }
}
