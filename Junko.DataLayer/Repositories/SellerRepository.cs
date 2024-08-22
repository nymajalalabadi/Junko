using Junko.DataLayer.Context;
using Junko.Domain.Entities.Store;
using Junko.Domain.InterFaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.DataLayer.Repositories
{
    public class SellerRepository : ISellerRepository
    {
        #region constractor

        private readonly JunkoDbContext _context;

        public SellerRepository(JunkoDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        public async Task<IQueryable<Seller>> GetSellerQuery()
        {
            return _context.Sellers
                .Include(s => s.User)
                .AsQueryable();
        }

        public async Task<Seller?> GetSellerById(long id)
        {
            return await _context.Sellers.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> HasUnderProgressRequest(long userId)
        {
            return await _context.Sellers.AsQueryable()
                .AnyAsync(s => s.UserId == userId && s.StoreAcceptanceState == StoreAcceptanceState.UnderProgress);
        }

        public async Task<Seller?> GetLastActiveSellerByUserId(long userId)
        {
            return await _context.Sellers.AsQueryable()
                .OrderByDescending(s => s.CreateDate)
                .FirstOrDefaultAsync(s => s.UserId == userId && s.StoreAcceptanceState == StoreAcceptanceState.Accepted);
        }

        public async Task<bool> HasUserAnyActiveSellerPanel(long userId)
        {
            return await _context.Sellers.AsQueryable()
                .OrderByDescending(s => s.CreateDate)
                .AnyAsync(s => s.UserId == userId && s.StoreAcceptanceState == StoreAcceptanceState.Accepted);
        }

        public async Task AddSeller(Seller seller)
        {
            await _context.Sellers.AddAsync(seller);
        }

        public void UpdateSeller(Seller seller)
        {
            _context.Sellers.Update(seller);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
