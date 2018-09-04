﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;

namespace SportsStore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        // The ConfigureServices method is used to set up shared objects that can be used throughout the application through the dependency injection feature
        public void ConfigureServices(IServiceCollection services)
        {
            // Tells ASP.NET Core that when a component, such as a controller, needs an implementation of the IProductRepository interface, it should receive an instance of the FakeProductRepository class. The AddTransient method specifies that a new FakeProductRepository object should be created each time the IProductRepository interface is needed.
            services.AddTransient<IProductRepository, FakeProductRepository>();
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
