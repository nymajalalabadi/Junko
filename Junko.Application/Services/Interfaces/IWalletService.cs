using Junko.Domain.ViewModels.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Application.Services.Interfaces
{
    public interface IWalletService
    {
        #region Methods

        Task<FilterSellerWalletDTO> FilterSellerWallet(FilterSellerWalletDTO filter);

        #endregion
    }
}
