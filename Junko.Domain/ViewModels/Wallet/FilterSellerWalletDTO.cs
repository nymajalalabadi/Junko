using Junko.Domain.Entities.Wallet;
using Junko.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.ViewModels.Wallet
{
    public class FilterSellerWalletDTO : Paging<SellerWallet>
    {
        public long? SellerId { get; set; }

        public int? PriceFrom { get; set; }

        public int? PriceTo { get; set; }
    }
}
