using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Lab3_RPBDIS.Services;
using Lab3_RPBDIS.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Lab3_RPBDIS.Infrastructure;
using Lab3_RPBDIS.Models.Forms;
using Microsoft.EntityFrameworkCore;

namespace Lab3_RPBDIS
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add EF Core and DbContext configuration
            string connection = Configuration.GetConnectionString("RemoteSQLConnection");
            services.AddDbContext<AdvertisingAgencyDbContext>(options =>
                options.UseSqlServer(connection));

            // Add session support
            services.AddDistributedMemoryCache();
            services.AddSession();

            // Add cookie authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "MyCookie";
                    options.ExpireTimeSpan = TimeSpan.FromDays(30);
                });

            // Add memory caching
            services.AddMemoryCache();

            // Register services
            services.AddScoped<ICachedAdditionalServicesService, CachedAdditionalServicesService>();
            services.AddScoped<ICachedAdTypesService, CachedAdTypesService>();
            services.AddScoped<ICachedClientsService, CachedClientsService>();
            services.AddScoped<ICachedEmployeesService, CachedEmployeesService>();
            services.AddScoped<ICachedLocationsService, CachedLocationsService>();
            services.AddScoped<ICachedOrderServicesService, CachedOrderServicesService>();
            services.AddScoped<ICachedOrdersService, CachedOrdersService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();

            // Custom routing
            app.Map("/info", DisplayUserInfo);
            app.Map("/searchform1", DisplaySearchForm1);
            app.Map("/searchform2", DisplaySearchForm2);
            app.Map("/tables", tables =>
            {
                tables.Map("/AdTypes", DisplayAdTypes);
                tables.Map("/Clients", DisplayClients);
                tables.Map("/Employees", DisplayEmployees);
                tables.Map("/Locations", DisplayLocations);
                tables.Map("/Orders", DisplayOrders);

                tables.Run(async context =>
                {
                    string HTMLString = "<html><head><title>Tables</title><meta charset='utf-8'/></head>" +
                    "<body>" +
                    "<a href='/tables/AdTypes'>AdTypes</a><br>" +
                    "<a href='/tables/Clients'>Clients</a><br>" +
                    "<a href='/tables/Employees'>Employees</a><br>" +
                    "<a href='/tables/Locations'>Locations</a><br>" +
                    "<a href='/tables/Orders'>Orders</a><br>" +
                    "<a href='/tables'>Tables</a>" +
                    "<a href='/'>Home</a>" +
                    "</body></html>";
                    await context.Response.WriteAsync(HTMLString);
                });
            });

            app.Run(async context => {
                string HTMLString = "<html><head><title>Home</title><meta charset='utf-8'/></head>" +
                "<body>" +
                "<h1>Welcome</h1>" +
                "<a href='/info'>Info</a><br>" +
                "<a href='/tables'>Tables</a><br>" +
                "<a href='/searchform1'>Search Form 1</a><br>" +
                "<a href='/searchform2'>Search Form 2</a>" +
                "</body></html>";
                await context.Response.WriteAsync(HTMLString);
            });
        }

        private static void DisplayUserInfo(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                string response = "<html><head><title>Info</title></head>" +
                "<body><h1>Client Info:</h1>" +
                "<p>Server: " + context.Request.Host + "</p>" +
                "<p>Path: " + context.Request.PathBase + "</p>" +
                "<p>Protocol: " + context.Request.Protocol + "</p>" +
                "<a href='/'>Home</a></body></html>";
                await context.Response.WriteAsync(response);
            });
        }

        private static void DisplayAdTypes(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                ICachedAdTypesService cachedService = context.RequestServices.GetService<ICachedAdTypesService>();
                var adTypes = cachedService.GetAdTypes("AdTypes20");

                string html = "<html><head><title>Ad Types</title></head>" +
                "<body><h1>Ad Types</h1><table border='1'><tr><th>ID</th><th>Name</th></tr>";

                foreach (var adType in adTypes)
                {
                    html += $"<tr><td>{adType.AdTypeId}</td><td>{adType.Name}</td></tr>";
                }

                html += "</table><a href='/'>Home</a></body></html>";
                await context.Response.WriteAsync(html);
            });
        }


        private static void DisplayClients(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                ICachedClientsService cachedService = context.RequestServices.GetService<ICachedClientsService>();
                var clients = cachedService.GetClients("Clients20");

                string html = "<html><head><title>Clients</title></head>" +
                "<body><h1>Clients</h1><table border='1'><tr><th>ID</th><th>Name</th></tr>";

                foreach (var client in clients)
                {
                    html += $"<tr><td>{client.ClientId}</td><td>{client.FirstName} {client.LastName}</td></tr>";
                }

                html += "</table><a href='/'>Home</a></body></html>";
                await context.Response.WriteAsync(html);
            });
        }

        private static void DisplayEmployees(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                ICachedEmployeesService cachedService = context.RequestServices.GetService<ICachedEmployeesService>();
                var employees = cachedService.GetEmployees("Employees20");

                string html = "<html><head><title>Employees</title></head>" +
                "<body><h1>Employees</h1><table border='1'><tr><th>ID</th><th>Name</th></tr>";

                foreach (var employee in employees)
                {
                    html += $"<tr><td>{employee.EmployeeId}</td><td>{employee.FirstName} {employee.LastName}</td></tr>";
                }

                html += "</table><a href='/'>Home</a></body></html>";
                await context.Response.WriteAsync(html);
            });
        }

        private static void DisplayLocations(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                ICachedLocationsService cachedService = context.RequestServices.GetService<ICachedLocationsService>();
                var locations = cachedService.GetLocations("Locations20");

                string html = "<html><head><title>Locations</title></head>" +
                "<body><h1>Locations</h1><table border='1'><tr><th>ID</th><th>Name</th></tr>";

                foreach (var location in locations)
                {
                    html += $"<tr><td>{location.LocationId}</td><td>{location.Name}</td></tr>";
                }

                html += "</table><a href='/'>Home</a></body></html>";
                await context.Response.WriteAsync(html);
            });
        }

        private static void DisplayOrders(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                ICachedOrdersService cachedService = context.RequestServices.GetService<ICachedOrdersService>();
                var orders = cachedService.GetOrders("Orders20");

                string html = "<html><head><title>Orders</title></head>" +
                "<body><h1>Orders</h1><table border='1'><tr>" +
                "<th>ID</th><th>Order Date</th><th>Start Date</th><th>End Date</th>" +
                "<th>Client Name</th><th>Location</th><th>Employee</th><th>Total Cost</th><th>Paid</th></tr>";
                //
                foreach (var order in orders)
                {
                    html += $"<tr><td>{order.OrderId}</td>" +
                            $"<td>{order.OrderDate?.ToString("yyyy-MM-dd")}</td>" +
                            $"<td>{order.StartDate?.ToString("yyyy-MM-dd")}</td>" +
                            $"<td>{order.EndDate?.ToString("yyyy-MM-dd")}</td>" +
                            $"<td>{order.Client?.FirstName} {order.Client?.LastName}</td>" +
                            $"<td>{order.Location?.Name}</td>" +
                            $"<td>{order.Employee?.FirstName} {order.Employee?.LastName}</td>" +
                            $"<td>{order.TotalCost:C}</td>" +
                            $"<td>{(order.Paid == true ? "Yes" : "No")}</td></tr>";
                }

                html += "</table><a href='/'>Home</a></body></html>";
                await context.Response.WriteAsync(html);
            });
        }

        private static void DisplaySearchForm1(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                CookieOptions options = new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(5)
                };

                string adTypeName = context.Request.Cookies["adTypeName"] ?? "";
                string adTypeDescription = context.Request.Cookies["adTypeDescription"] ?? "";

                string html = "<html><head><meta charset='utf-8'></head><body>" +
                "<form method='get'>" +
                "Ad Type Name:<br><input type='text' name='adTypeName' value='" + adTypeName + "'><br>" +
                "Ad Type Description:<br><input type='text' name='adTypeDescription' value='" + adTypeDescription + "'><br>" +
                "<input type='submit' value='Submit'>" +
                "</form><a href='/'>Home</a></body></html>";

                if (context.Request.Query.ContainsKey("adTypeName"))
                {
                    context.Response.Cookies.Append("adTypeName", context.Request.Query["adTypeName"], options);
                    context.Response.Cookies.Append("adTypeDescription", context.Request.Query["adTypeDescription"], options);
                }

                await context.Response.WriteAsync(html);
            });
        }


        private static void DisplaySearchForm2(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                var form = context.Session.Get<SearchForm2>("form") ?? new SearchForm2();

                string html = "<html><head><meta charset='utf-8'></head><body>" +
                "<form method='get'>" +
                "Ad Type Name:<br><input type='text' name='adTypeName' value='" + form.AdTypeName + "'><br>" +
                "Ad Type Description:<br><input type='text' name='adTypeDescription' value='" + form.AdTypeDescription + "'><br>" +
                "<input type='submit' value='Submit'>" +
                "</form><a href='/'>Home</a></body></html>";

                form.AdTypeName = context.Request.Query["adTypeName"];
                form.AdTypeDescription = context.Request.Query["adTypeDescription"];
                context.Session.Set("form", form);

                await context.Response.WriteAsync(html);
            });
        }
    }
}