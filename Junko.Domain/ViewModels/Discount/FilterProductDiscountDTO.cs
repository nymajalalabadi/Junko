using Junko.Domain.Entities.Discount;
using Junko.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.ViewModels.Discount
{
    public class FilterProductDiscountDTO : Paging<ProductDiscount>
    {
        #region properties

        public long? ProductId { get; set; }

        public long? SellerId { get; set; }

        #endregion
    }
}
