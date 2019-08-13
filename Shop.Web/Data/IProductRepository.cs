using System.Linq;

namespace Shop.Web.Data
{
    using Entities;

    public interface IProductRepository : IGenericRepository<Product>
    {
        IQueryable GetAllWithUsers();
    }
}