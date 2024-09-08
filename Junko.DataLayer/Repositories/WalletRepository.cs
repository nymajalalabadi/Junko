using Junko.DataLayer.Context;
using Junko.Domain.InterFaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.DataLayer.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        #region Constructor

        private JunkoDbContext _context;

        public WalletRepository(JunkoDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods



        #endregion
    }
}
