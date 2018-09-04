using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;

        // When MVC needs to create a new instance of the ProductController class to handle an HTTP request, it will inspect the constructor and see that it requires an object that implements the IProductRepository interface. To determine what implementation class should be used, MVC consults the configuration in the Startup class, which tells it that FakeRepository should be used and that a new instance should be created every time. MVC creates a new FakeRepository object and uses it to invoke the ProductController constructor in order to create the controller object that will process the HTTP request.
        // This is known as dependency injection, and its approach allows the ProductController constructor to access the application’s repository through the IProductRepository interface without having any need to know which implementation class has been configured. Later, I’ll replace the fake repository with the real one, and dependency injection means that the controller will continue to work without changes.
        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }

        //Will render a view showing the complete list of the products in the repository
        //Calling the View method like this (without specifying a view name) tells MVC to render the default view for the action method. Passing the collection of Product objects from the repository to the View method provides the framework with the data with which to populate the Model object in a strongly typed view.
        public ViewResult List() => View(repository.Products);
    }
}
