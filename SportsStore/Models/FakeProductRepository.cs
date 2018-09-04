using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    // The FakeProductRepository class implements the IProductRepository interface by returning a fixed collection of Product objects as the value of the Products property. The AsQueryable method is used to convert the fixed collection of objects to an IQueryable<Product>, which is required to implement the IProductRepository interface and allows me to create a compatible fake repository without having to deal with real queries.
    public class FakeProductRepository : IProductRepository
    {
        public IQueryable<Product> Products => new List<Product>
        {
            new Product { Name = "Football", Price = 25 },
            new Product { Name = "Surf board", Price = 179 },
            new Product { Name = "Running shoes", Price = 95 },
        }.AsQueryable<Product>();
    }
}
