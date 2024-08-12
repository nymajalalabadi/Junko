using Junko.Domain.Entities.Account;
using Junko.Domain.Entities.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.InterFaces
{
    public interface ISellerRepository
    {
        #region Methods

        Task<IQueryable<Seller>> GetSellerQuery();

        Task<Seller?> GetSellerById(long id);

        Task<bool> HasUnderProgressRequest(long userId);

        Task AddSeller(Seller seller);

        void UpdateSeller(Seller seller);

        Task SaveChanges();

        #endregion
    }
}
