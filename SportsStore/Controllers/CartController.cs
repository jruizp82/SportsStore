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
        public CartController(IProductRepository repo)
        {
            repository = repo;
        }

        //The Index action retrieves the Cart object from the session state and uses it to create a CartIndexView
        //Model object, which is then passed to the View method to be used as the view model.
        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),
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
                Cart cart = GetCart();
                cart.AddItem(product, 1);
                SaveCart(cart);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                Cart cart = GetCart();
                cart.RemoveLine(product);
                SaveCart(cart);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        //use the ASP.NET session state feature to store and retrieve Cart objects

        //The HttpContext property is provided the Controller base class from which controllers are usually derived
        //and returns an HttpContext object that provides context data about the request that has been received and the
        //response that is being prepared.The HttpContext.Session property returns an object that implements the
        //ISession interface, which is the type on which I defined the SetJson method, which accepts arguments that
        //specify a key and an object that will be added to the session state.The extension method serializes the object and
        //adds it to the session state using the underlying functionality provided by the ISession interface.

        private Cart GetCart()
        {
            //To retrieve the Cart again
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }

        private void SaveCart(Cart cart)
        {
            //To add a Cart to the session state in the controller
            HttpContext.Session.SetJson("Cart", cart);
        }

        //The middleware that I registered in the previous section uses cookies or URL rewriting to associate multiple requests from a user
        //together to form a single browsing session.A related feature is session state, which associates data with a
        //session. This is an ideal fit for the Cart class: I want each user to have their own cart, and I want the cart to
        //be persistent between requests.Data associated with a session is deleted when a session expires (typically
        //because a user has not made a request for a while), which means that I do not need to manage the storage or
        //life cycle of the Cart objects.
    }
}
