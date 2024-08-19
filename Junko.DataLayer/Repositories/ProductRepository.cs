using Junko.DataLayer.Context;
using Junko.Domain.Entities.Account;
using Junko.Domain.Entities.Products;
using Junko.Domain.InterFaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

        #region Product

        public async Task<IQueryable<Product>> GetProductQuery()
        {
            return _context.Products
                .Include(p => p.Seller)
                .Where(p => !p.IsDelete)
                .AsQueryable();
        }

        public async Task AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        #endregion

        #region Product Category

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

        public async Task AddRangeProductSelectedCategorys(List<ProductSelectedCategory> productSelectedCategories)
        {
            foreach (var productSelectedCategory in productSelectedCategories)
            {
                await _context.ProductSelectedCategories.AddAsync(productSelectedCategory);
            }
        }

        #endregion

        #region Product Color

        public async Task AddRangeProductColors(List<ProductColor> productColors)
        {
            foreach (var color in productColors)
            {
                await _context.ProductColors.AddAsync(color);
            }
        }

        #endregion

        #region Product Size

        public async Task AddRangeProductSizes(List<ProductSize> productSizes)
        {
            foreach (var size in productSizes)
            {
                await _context.ProductSizes.AddAsync(size);
            }
        }

        #endregion


        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
