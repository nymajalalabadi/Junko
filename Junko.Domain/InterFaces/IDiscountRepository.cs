using Junko.Domain.Entities.Discount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.InterFaces
{
    public interface IDiscountRepository
    {
        #region Methods

        #region Product Discoun

        Task<IQueryable<ProductDiscount>> GetAllProductDiscounts();

        Task AddProductDiscount(ProductDiscount productDiscount);

        #endregion

        #region Product Discoun Use

        Task AddDiscountProductUse(ProductDiscountUse productDiscountUse);

        #endregion

        Task SaveChanges();

        #endregion
    }
}
