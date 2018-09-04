using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public interface IProductRepository
    {
        /*The IQueryable<T> interface is useful because it allows a collection of objects to be queried efficiently. later in this chapter, I add support for retrieving a subset of Product objects from a database, and using the IQueryable<T> interface allows me to ask the database for just the objects that I require using standard lINQ statements and without needing to know what database server stores the data or how it processes the query. 
         *Without the IQueryable<T> interface, I would have to retrieve all of the Product objects from the database and then discard the ones I don’t want, which becomes an expensive operation as the amount of data used by an application increases. It is for this reason that the IQueryable<T> interface is typically used instead of IEnumerable<T> in database repository interfaces and classes.
         *However, care must be taken with the IQueryable<T> interface because each time the collection of objects is enumerated, the query will be evaluated again, which means that a new query will be sent to the database. This can undermine the efficiency gains of using IQueryable<T>. In such situations, you can convert IQueryable<T> to a more predictable form using the ToList or ToArray extension method.
         */
        IQueryable<Product> Products { get; }
    }
}
