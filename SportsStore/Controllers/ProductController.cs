using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4; // specifies that I want four products per page

        // When MVC needs to create a new instance of the ProductController class to handle an HTTP request, it will inspect the constructor and see that it requires an object that implements the IProductRepository interface. To determine what implementation class should be used, MVC consults the configuration in the Startup class, which tells it that FakeRepository should be used and that a new instance should be created every time. MVC creates a new FakeRepository object and uses it to invoke the ProductController constructor in order to create the controller object that will process the HTTP request.
        // This is known as dependency injection, and its approach allows the ProductController constructor to access the application’s repository through the IProductRepository interface without having any need to know which implementation class has been configured. Later, I’ll replace the fake repository with the real one, and dependency injection means that the controller will continue to work without changes.
        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }

        //Will render a view showing the complete list of the products in the repository
        //Calling the View method like this (without specifying a view name) tells MVC to render the default view for the action method. Passing the collection of Product objects from the repository to the View method provides the framework with the data with which to populate the Model object in a strongly typed view.
        //I have added an optional parameter to the List method, which means that if I call the method without a parameter (List()), my call is treated as though I had supplied the value specified in the parameter definition (List(1)). The effect is that the action method displays the first page of products when MVC invokes it without an argument. Within the body of the action method, I get the Product objects, order them by the primary key, skip over the products that occur before the start of the current page, and take the number of products specified by the PageSize field. 
        //update the List action method in the ProductController class to use the 
        //ProductsListViewModel class to provide the view with details of the products to display on the page and   
        //details of the pagination

        //to update the Product controller so that the List action method will filter Product objects by category and use the new property I added to the view
        //model to indicate which category has been selected.
        public ViewResult List(string category, int productPage = 1)
            => View(new ProductsListViewModel
            {
                Products = repository.Products
                    // if category is not null, only those Product objects with a matching Category property are selected.
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(p => p.ProductID)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Products.Count()
                },
                CurrentCategory = category
            });
        //These changes pass a ProductsListViewModel object as the model data to the view.

    }
}
