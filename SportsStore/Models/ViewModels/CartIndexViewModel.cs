using SportsStore.Models;
namespace SportsStore.Models.ViewModels
{
    public class CartIndexViewModel
    {
        //I need to pass two pieces of information to the view that will display the contents of the cart: the Cart
        //object and the URL to display if the user clicks the Continue Shopping button.
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
    }
}
