using Junko.DataLayer.Context;
using Junko.Domain.Entities.Products;
using Junko.Domain.InterFaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.DataLayer.Repositories
{
    public class ProductRepository : IProductRepository
    {
        #region constractor

        private readonly JunkoDbContext _context;

        public ProductRepository(JunkoDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        public async Task<IQueryable<Product>> GetProductQuery()
        {
            return _context.Products
                .Include(p => p.Seller)
                .Where(p => !p.IsDelete)
                .AsQueryable();
        }

        public async Task<List<ProductCategory>> GetAllProductCategories()
        {
            return await _context.ProductCategories.AsQueryable()
                .Where(c => !c.IsDelete && c.IsActive)
                .ToListAsync();
        }

        public async Task<List<ProductCategory>> GetAllProductCategoriesByParentId(long parentId)
        {
            return await _context.ProductCategories.AsQueryable()
                .Where(c => !c.IsDelete && c.IsActive && c.ParentId == parentId)
                .ToListAsync();
        }

        #endregion
    }
}
