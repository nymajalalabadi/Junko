﻿using Junko.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.Entities.Products
{
    public class ProductSelectedCategory : BaseEntity
    {
        #region properties

        public long ProductId { get; set; }

        public long ProductCategoryId { get; set; }

        #endregion

        #region relations

        public Product Product { get; set; }

        public ProductCategory ProductCategory { get; set; }

        #endregion
    }

}
