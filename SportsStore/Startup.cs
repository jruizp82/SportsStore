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
            services.AddMvc(); // sets up the shared objects used in MVC applications
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

            // enables ASP.NET Core MVC
            // The UseMvc method sets up the MVC middleware, and one of the configuration options is the scheme that will be used to map URLs to controllers and action method
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    // tells MVC to send requests to the List action method of the Product controller unless the request URL specifies otherwise
                    template: "{controller=Product}/{action=List}/{id?}");                
            });
        }
    }
}
