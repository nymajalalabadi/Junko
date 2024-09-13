using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.ViewModels.Orders
{
    public class UserOpenOrderDetailItemDTO
    {
        public long DetailId { get; set; }

        public long ProductId { get; set; }

        public string ProductTitle { get; set; }

        public string ProductImageName { get; set; }

        public int Count { get; set; }

        public long? ProductColorId { get; set; }

        public long? ProductSizeId { get; set; }

        public int ProductPrice { get; set; }

        public int ProductColorPrice { get; set; }

        public string ColorName { get; set; }

        public string size { get; set; }

        public int? DiscountPercentage { get; set; }

        #region Discount

        public int GetOrderDetailWithDiscountPriceAmount()
        {
            if (this.DiscountPercentage != null)
            {
                return (this.ProductPrice + this.ProductColorPrice) * this.DiscountPercentage.Value / 100 * this.Count;
            }

            return 0;
        }

        public string GetOrderDetailWithDiscountPrice()
        {
            if (this.DiscountPercentage != null)
            {
                return this.GetOrderDetailWithDiscountPriceAmount().ToString("#,0 تومان");
            }

            return "------";
        }

        #endregion

        public int GetTotalAmountByDiscount()
        {
            return (ProductPrice + ProductColorPrice) * Count - this.GetOrderDetailWithDiscountPriceAmount();
        }
    }
}
