using Junko.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.Entities.Products
{
    public class ProductSelectedColorSize : BaseEntity
    {
        #region properties

        public long ProductId { get; set; }

        public long ProductColorId { get; set; }

        public long ProductSizeId { get; set; }

        #endregion

        #region relations

        public Product Product { get; set; }

        public ProductColor ProductColor { get; set; }

        public ProductSize ProductSize { get; set; }

        #endregion
    }
}
