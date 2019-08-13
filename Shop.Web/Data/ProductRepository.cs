namespace Shop.Web.Data
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Entities;

    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Products.Include(x => x.User);
        }
    }
}