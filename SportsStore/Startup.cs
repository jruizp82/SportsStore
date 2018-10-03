using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace SportsStore
{
    public class Startup
    {
        // receives the configuration data loaded from the appsettings.json file, which is presented through an object that implements the IConfiguration interface. The constructor assigns the IConfiguration object to a property called Configuration so that it can be used by the rest of the Startup class
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        // The ConfigureServices method is used to set up shared objects that can be used throughout the application through the dependency injection feature
        public void ConfigureServices(IServiceCollection services)
        {
            // sets up the services provided by Entity Framework Core for the database context class.
            //  configured the database with the UseSqlServer method and specified the connection string, which is obtained from the Configuration property.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration["Data:SportsStoreProducts:ConnectionString"]));
            // The components in the application that use the IProductRepository interface, which is just the Product controller at the moment, will receive an EFProductRepository object when they are created, which will provide them with access to the data in the database
            services.AddTransient<IProductRepository, EFProductRepository>();
            
            //The AddScoped method specifies that the same object should be used to satisfy related requests for Cart
            //instances.How requests are related can be configured, but by default, it means that any Cart required by
            //components handling the same HTTP request will receive the same object.
            //Rather than provide the AddScoped method with a type mapping, as I did for the repository, I have
            //specified a lambda expression that will be invoked to satisfy Cart requests.The expression receives the
            //collection of services that have been registered and passes the collection to the GetCart method of the
            //SessionCart class. The result is that requests for the Cart service will be handled by creating SessionCart
            //objects, which will serialize themselves as session data when they are modified.
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));

            //specifies that the same object should always be used. The service I created tells MVC to use the HttpContextAccessor class when
            //implementations of the IHttpContextAccessor interface are required.This service is required so I can
            //access the current session in the SessionCart class
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //registered the order repository as a service in the ConfigureServices method
            services.AddTransient<IOrderRepository, EFOrderRepository>();
            services.AddMvc(); // sets up the shared objects used in MVC applications
            services.AddMemoryCache(); //call sets up the in-memory data store
            services.AddSession(); //registers the services used to access session data
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // The Configure method is used to set up the features that receive and process HTTP requests.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // displays details of exceptions that occur in the application, which is useful during the development process. It should not be enabled in deployed applications
            app.UseDeveloperExceptionPage();

            // adds a simple message to HTTP responses that would not otherwise have a body, such as 404 - Not Found responses
            app.UseStatusCodePages();

            // enables support for serving static content from the wwwroot folder, ej. pdf
            app.UseStaticFiles();

            //allows the session system to automatically associate requests with sessions when they arrive from the client
            app.UseSession();

            // enables ASP.NET Core MVC
            // The UseMvc method sets up the MVC middleware, and one of the configuration options is the scheme that will be used to map URLs to controllers and action method

            //URL Leads To
            ///Lists the first page of products from all categories
            ///Page2 Lists the specified page(in this case, page 2), showing items from all categories
            ///Soccer Shows the first page of items from a specific category(in this case, the Soccer category)
            ///Soccer/Page2 Shows the specified page(in this case, page 2) of items from the specified category (in this case, Soccer)
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: null,
                    template: "{category}/Page{productPage:int}",
                    defaults: new { controller = "Product", action = "List" }
                );

                routes.MapRoute(
                    name: null,
                    template: "Page{productPage:int}",
                    defaults: new { controller = "Product", action = "List", productPage = 1 }
                );

                routes.MapRoute(
                    name: null,
                    template: "{category}",
                    defaults: new { controller = "Product", action = "List", productPage = 1 }
                );

                routes.MapRoute(
                    name: null,
                    template: "",
                    defaults: new { controller = "Product", action = "List", productPage = 1 }
                );

                routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
            });
            //to seed the database when the application starts, which I have done by adding a call to the EnsurePopulated method from the Startup class
            SeedData.EnsurePopulated(app); 
        }
    }
}
