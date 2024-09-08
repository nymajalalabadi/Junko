using Junko.Application.Services.Interfaces;
using Junko.Domain.InterFaces;
using Junko.Domain.ViewModels.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Application.Services.Implementations
{
    public class WalletService : IWalletService
    {
        #region Constructor

        private readonly IWalletRepository _walletRepository;

        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        #endregion

        #region Methods

        public async Task<FilterSellerWalletDTO> FilterSellerWallet(FilterSellerWalletDTO filter)
        {
            var query = await _walletRepository.GetAllSellerWallets();

            if (filter.SellerId != null && filter.SellerId != 0)
            {
                query = query.Where(s => s.SellerId.Equals(filter.SellerId));
            }

            if (filter.PriceFrom != null)
            {
                query = query.Where(s => s.Price >= filter.PriceFrom.Value);
            }

            if (filter.PriceTo != null)
            {
                query = query.Where(s => s.Price <= filter.PriceTo.Value);
            }

            query = query.OrderByDescending(w => w.CreateDate);

            #region paging

            await filter.SetPaging(query);

            #endregion

            return filter;
        }

        #endregion
    }
}
