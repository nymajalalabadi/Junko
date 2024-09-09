using Junko.Domain.Entities.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.InterFaces
{
    public interface IWalletRepository
    {
        #region Methods

        Task<IQueryable<SellerWallet>> GetAllSellerWallets();

        Task AddWallet(SellerWallet wallet);

        Task SaveChanges();

        #endregion
    }
}
