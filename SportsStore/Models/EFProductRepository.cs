using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    // for the moment, the repository implementation just maps the Products property defined by the IProductRepository interface onto the Products property defined by the ApplicationDbContext class. The Products property in the context class returns a DbSet<Product> object, which implements the IQueryable<T> interface and makes it easy to implement the IProductRepository interface when using Entity Framework Core. This ensures that queries to the database will retrieve only the objects that are required
    public class EFProductRepository : IProductRepository
    {
        private ApplicationDbContext context;

        public EFProductRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Product> Products => context.Products;
    }
}
