using Junko.Domain.Entities.Products;
using Junko.Domain.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public string? FilterByCategory { get; set; }

        #region price

        public int FilterMinPrice { get; set; }

        public int FilterMaxPrice { get; set; }

        public int SelectedMinPrice { get; set; }

        public int SelectedMaxPrice { get; set; }

        public int PriceStep { get; set; } = 100000;

        #endregion

        public FilterProductState FilterProductState { get; set; }

        public FilterProductOrderBy FilterProductOrderBy { get; set; }

        public List<long> SelectedProductCategories { get; set; }

        #endregion
    }

    public enum FilterProductState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "در حال بررسی")]
        UnderProgress,
        [Display(Name = "تایید شده")]
        Accepted,
        [Display(Name = "رد شده")]
        Rejected,
        [Display(Name = "فعال")]
        Active,
        [Display(Name = "غیر فعال")]
        NotActive
    }


    public enum FilterProductOrderBy
    {
        [Display(Name = "تاریخ ( نزولی )")]
        CreateData_Des,
        [Display(Name = "تاریخ ( صعودی )")]
        CreateDate_Asc,
        [Display(Name = "قیمت ( نزولی )")]
        Price_Des,
        [Display(Name = "قیمت ( صعودی )")]
        Price_Asc,
    }

}
