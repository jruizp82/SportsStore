using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        //I need to modify the constructor so that it receives the services it
        //requires to process an order
        public OrderController(IOrderRepository repoService, Cart cartService)
        {
            repository = repoService;
            cart = cartService;
        }

        //The Checkout method returns the default view and passes a new ShippingDetails object as the view
        //model.
        public ViewResult Checkout() => View(new Order());

        //The Checkout action method is decorated with the HttpPost attribute, which means that it will be
        //invoked for a POST request—in this case, when the user submits the form.Once again, I am relying on the
        //model binding system so that I can receive the Order object, which I then complete using data from the Cart
        //and store in the repository.
        //MVC checks the validation constraints that I applied to the Order class using the data annotation
        //attributes, and any validation problems are passed to the action method through the ModelState property.
        //I can see whether there are any problems by checking the ModelState.IsValid property. I call the
        //ModelState.AddModelError method to register an error message if there are no items in the cart
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }

        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }
    }
}
