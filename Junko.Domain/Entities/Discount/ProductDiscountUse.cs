using Junko.Domain.Entities.Account;
using Junko.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.Entities.Discount
{
    public class ProductDiscountUse : BaseEntity
    {
        #region properties

        public long ProductDiscountId { get; set; }

        public long UserId { get; set; }

        #endregion

        #region relations

        public User User { get; set; }
        public ProductDiscount ProductDiscount { get; set; }

        #endregion
    }
}
