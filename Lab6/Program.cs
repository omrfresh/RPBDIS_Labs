using Lab6.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Reflection;

namespace Lab6
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Запись действий в журнал с использованием пакета Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.File("Lab6Log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            var services = builder.Services;

            // Настройка контекста базы данных
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AdvertisingDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Добавление сервисов
            services.AddControllers();

            // Настройка Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Advertising API",
                    Description = "Данные о рекламных кампаниях",
                    Contact = new OpenApiContact
                    {
                        Name = "Your Name",
                        Email = string.Empty,
                        Url = new Uri("https://github.com/your-repo")
                    }
                });

                // Включение комментариев XML
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            var app = builder.Build();

            // Использование Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Advertising API V1");
            });

            app.UseDeveloperExceptionPage();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Инициализация базы данных
            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var context = serviceProvider.GetRequiredService<AdvertisingDbContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception exception)
                {
                    Log.Fatal(exception, "An error occurred while db initialization");
                }
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // Настройка маршрутов для HTML-страниц
            app.MapGet("/fetch_clients", async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync("wwwroot/fetch_clients.html");
            });

            app.MapGet("/jq_clients", async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync("wwwroot/jq_clients.html");
            });

            app.MapGet("/fetch_adtypes", async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync("wwwroot/fetch_adtypes.html");
            });

            app.MapGet("/jq_adtypes", async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync("wwwroot/jq_adtypes.html");
            });

            app.MapGet("/fetch_locations", async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync("wwwroot/fetch_locations.html");
            });

            app.MapGet("/jq_locations", async context =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.SendFileAsync("wwwroot/jq_locations.html");
            });

            app.Run();
        }
    }
}
