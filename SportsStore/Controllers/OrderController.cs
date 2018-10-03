using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        //The Checkout method returns the default view and passes a new ShippingDetails object as the view
        //model.
        public ViewResult Checkout() => View(new Order());
    }
}
