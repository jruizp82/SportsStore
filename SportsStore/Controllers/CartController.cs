using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Infrastructure;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        private Cart cart;

        //The CartController class indicates that it needs a Cart object by declaring a constructor argument,
        //which has allowed me to remove the methods that read and write data from the session and the steps
        //required to write updates.The result is a controller that is simpler and remains focused on its role in the
        //application without having to worry about how Cart objects are created or persisted. And, since services are
        //available throughout the application, any component can get hold of the user’s cart using the same technique.
        public CartController(IProductRepository repo, Cart cartService)
        {
            repository = repo;
            cart = cartService;
        }

        //The Index action retrieves the Cart object from the session state and uses it to create a CartIndexView
        //Model object, which is then passed to the View method to be used as the view model.
        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        //For the AddToCart and RemoveFromCart action methods, I have used parameter names that match
        //the input elements in the HTML forms created in the ProductSummary.cshtml view.This allows MVC to
        //associate incoming form POST variables with those parameters, meaning I do not need to process the form
        //myself.This is known as model binding and is a powerful tool for simplifying controller classes
        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }      

    }
}
