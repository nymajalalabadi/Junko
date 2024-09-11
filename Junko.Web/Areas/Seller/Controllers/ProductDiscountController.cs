using Junko.Application.Extensions;
using Junko.Application.Services.Interfaces;
using Junko.Domain.ViewModels.Discount;
using Microsoft.AspNetCore.Mvc;

namespace Junko.Web.Areas.Seller.Controllers
{
    public class ProductDiscountController : SellerBaseController
    {
        #region constructor

        private readonly IDiscountService _discountService;
        private readonly ISellerService _sellerService;

        public ProductDiscountController(IDiscountService discountService, ISellerService sellerService)
        {
            _discountService = discountService;
            _sellerService = sellerService;
        }

        #endregion

        #region filter discount

        [HttpGet("discounts")]
        [HttpGet("discounts/{ProductId}")]
        public async Task<IActionResult> FilterDiscounts(FilterProductDiscountDTO filter)
        {
            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
            filter.SellerId = seller.Id;

            var result = await _discountService.FilterProductDiscount(filter);

            return View(result);
        }

        #endregion

    }
}
