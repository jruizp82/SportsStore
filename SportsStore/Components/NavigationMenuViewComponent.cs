using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IProductRepository repository;

        //defines an IProductRepository argument.When MVC needs to
        //create an instance of the view component class, it will note the need to provide this argument and inspect
        //the configuration in the Startup class to determine which implementation object should be used.
        public NavigationMenuViewComponent(IProductRepository repo)
        {
            repository = repo;
        }

        //In the Invoke method, I use LINQ to select and order the set of categories in the repository and pass
        //them as the argument to the View method, which renders the default Razor partial view, details of which
        //are returned from the method using an IViewComponentResult object
        public IViewComponentResult Invoke()
        {
            //Inside the Invoke method, I have dynamically assigned a SelectedCategory property to the ViewBag
            //object and set its value to be the current category, which is obtained through the context object returned
            //by the RouteData property.
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repository.Products
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x));
        }
    }
}
