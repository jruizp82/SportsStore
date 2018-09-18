using System.Linq;
using SportsStore.Models;
using Xunit;
namespace SportsStore.Tests
{
    public class CartTests
    {
        //the first behavior relates to when i add an item to the cart. if this is the first time that a given Product 
        //has been added to the cart, i want a new CartLine to be added
        [Fact]
        public void Can_Add_New_Lines()
        {
            // Arrange - create some test products
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            
            // Arrange - create a new cart
            Cart target = new Cart();
            
            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] results = target.Lines.ToArray();
            
            // Assert
            Assert.Equal(2, results.Length);
            Assert.Equal(p1, results[0].Product);
            Assert.Equal(p2, results[1].Product);
        }

        //however, if the customer has already added a Product to the cart, i want to increment the quantity of
        //the corresponding CartLine and not create a new one
        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Arrange - create some test products
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            
            // Arrange - create a new cart
            Cart target = new Cart();
            
            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            CartLine[] results = target.Lines
                .OrderBy(c => c.Product.ProductID).ToArray();
            
            // Assert
            Assert.Equal(2, results.Length);
            Assert.Equal(11, results[0].Quantity);
            Assert.Equal(1, results[1].Quantity);
        }

        //I also need to check that users can change their mind and remove products from the cart.
        [Fact]
        public void Can_Remove_Line()
        {
            // Arrange - create some test products
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };
            
            // Arrange - create a new cart
            Cart target = new Cart();
            
            // Arrange - add some products to the cart
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);
            
            // Act
            target.RemoveLine(p2);
            
            // Assert
            Assert.Empty(target.Lines.Where(c => c.Product == p2));
            Assert.Equal(2, target.Lines.Count());
        }

        //The next behavior i want to test is the ability to calculate the total cost of the items in the cart
        [Fact]
        public void Calculate_Cart_Total()
        {
            // Arrange - create some test products
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
            
            // Arrange - create a new cart
            Cart target = new Cart();
            
            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();
            
            // Assert
            Assert.Equal(450M, result);
        }

        //the final test is simple.i want to ensure that the contents of the cart are properly removed when reset
        [Fact]
        public void Can_Clear_Contents()
        {
            // Arrange - create some test products
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
            
            // Arrange - create a new cart
            Cart target = new Cart();
            
            // Arrange - add some items
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            
            // Act - reset the cart
            target.Clear();
            
            // Assert
            Assert.Empty(target.Lines);
        }

    }
}