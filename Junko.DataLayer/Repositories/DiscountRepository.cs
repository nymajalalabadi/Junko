using Junko.DataLayer.Context;
using Junko.Domain.Entities.Discount;
using Junko.Domain.InterFaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.DataLayer.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        #region constractor

        private readonly JunkoDbContext _context;

        public DiscountRepository(JunkoDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        #region Product Discoun

        public async Task<IQueryable<ProductDiscount>> GetAllProductDiscounts()
        {
            return _context.ProductDiscounts
                .Include(d => d.Product)
                .Where(d => !d.IsDelete).AsQueryable();
        }

        public async Task AddProductDiscount(ProductDiscount productDiscount)
        {
            await _context.ProductDiscounts.AddAsync(productDiscount);
        }

        #endregion

        #region Product Discoun Use

        public async Task AddDiscountProductUse(ProductDiscountUse productDiscountUse)
        {
            await _context.productDiscountUses.AddAsync(productDiscountUse);
        }

        #endregion

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
