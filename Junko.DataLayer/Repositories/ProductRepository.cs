﻿using Junko.DataLayer.Context;
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

        #endregion
    }
}
