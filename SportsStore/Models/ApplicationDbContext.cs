using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace SportsStore.Models
{
    //The DbContext base class provides access to the Entity Framework Core’s underlying functionality, and the Products property will provide access to the Product objects in the database. The ApplicationDbContext class is derived from DbContext and adds the properties that will be used to read and write the application’s data. There is only one property at the moment, which will provide access to Product objects.
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}
