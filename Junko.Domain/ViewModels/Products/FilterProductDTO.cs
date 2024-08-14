using Junko.Domain.Entities.Products;
using Junko.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.ViewModels.Products
{
    public class FilterProductDTO : Paging<Product>
    {
        #region properteis

        public long? SellerId { get; set; }

        public string? ProductTitle { get; set; }

        public FilterProductState FilterProductState { get; set; }


        #endregion
    }

    public enum FilterProductState
    {
        UnderProgress,
        Accepted,
        Rejected,
        Active,
        NotActive
    }

}
