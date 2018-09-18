using System.Collections.Generic;
using System.Linq;
namespace SportsStore.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        //add an item to the cart
        public virtual void AddItem(Product product, int quantity)
        {
            CartLine line = lineCollection
                .Where(p => p.Product.ProductID == product.ProductID)
                .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        //remove a previously added item from the cart
        public virtual void RemoveLine(Product product) =>
            lineCollection.RemoveAll(l => l.Product.ProductID == product.ProductID);

        //calculate the total cost of the items in the cart
        public virtual decimal ComputeTotalValue() =>
            lineCollection.Sum(e => e.Product.Price * e.Quantity);

        //reset the cart by removing all the items
        public virtual void Clear() => lineCollection.Clear();

        //property that gives access to the contents of the cart using an IEnumerable<CartLine>
        public virtual IEnumerable<CartLine> Lines => lineCollection;
    }

    //The Cart class uses the CartLine class, defined in the same file, to represent a product selected by
    //the customer and the quantity the user wants to buy.
    public class CartLine
    {
        public int CartLineID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
