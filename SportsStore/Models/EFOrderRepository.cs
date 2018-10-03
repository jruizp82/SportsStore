using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace SportsStore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private ApplicationDbContext context;
        public EFOrderRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        //the Include and ThenInclude methods to specify that when an Order object is read from the
        //database, the collection associated with the Lines property should also be loaded along with each
        //Product object associated with each collection object.
        public IQueryable<Order> Orders => context.Orders
                             .Include(o => o.Lines)
                             .ThenInclude(l => l.Product);
        //this ensures that i receive all the data objects that i need without having to perform the queries and
        //assemble the data directly.

        public void SaveOrder(Order order)
        {
            //notify entity Framework Core that the objects exist and
            //shouldn’t be stored in the database unless they are modified
            context.AttachRange(order.Lines.Select(l => l.Product));
            //this ensures that entity Framework Core won’t try to write the deserialized Product objects that are
            //associated with the Order object.
            if (order.OrderID == 0)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }

    }
}
