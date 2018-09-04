using System.Collections.Generic;
using SportsStore.Models;

namespace SportsStore.Models.ViewModels
{
    //send from the controller to the view in a single view model class 
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
