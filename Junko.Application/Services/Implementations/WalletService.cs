using Junko.Application.Services.Interfaces;
using Junko.Domain.InterFaces;
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



        #endregion
    }
}
