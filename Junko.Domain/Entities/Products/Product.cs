using Junko.Domain.Entities.Common;
using Junko.Domain.Entities.Discount;
using Junko.Domain.Entities.ProductOrder;
using Junko.Domain.Entities.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.Entities.Products
{
    public class Product : BaseEntity
    {
        #region properties

        public long SellerId { get; set; }

        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "تصویر محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ShortDescription { get; set; }

        [Display(Name = "توضیحات اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "پیام تایید/عدم تایید")]
        public string? ProductAcceptOrRejectDescription { get; set; }

        [Display(Name = "فعال / غیرفعال")]
        public bool IsActive { get; set; }

        [Display(Name = "درصد سایت")]
        public int SiteProfit { get; set; }

        [Display(Name = "وضعیت")]
        public ProductAcceptanceState ProductAcceptanceState { get; set; }

        #endregion

        #region relations

        public Seller Seller { get; set; }

        public ICollection<ProductSelectedCategory> ProductSelectedCategories { get; set; }

        public ICollection<ProductColor> ProductColors { get; set; }

        public ICollection<ProductSize> ProductSizes { get; set; }

        public ICollection<ProductGallery> ProductGalleries { get; set; }

        public ICollection<ProductFeature> ProductFeatures { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }

        public ICollection<ProductDiscount> ProductDiscounts { get; set; }

        #endregion
    }

    public enum ProductAcceptanceState
    {
        [Display(Name = "در حال بررسی")]
        UnderProgress,
        [Display(Name = "تایید شده")]
        Accepted,
        [Display(Name = "رد شده")]
        Rejected
    }

}
