using Junko.Application.Extensions;
using Junko.Application.Services.Interfaces;
using Junko.Domain.Entities.Store;
using Junko.Domain.ViewModels.Discount;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Junko.Web.Areas.Seller.Controllers
{
    public class ProductDiscountController : SellerBaseController
    {
        #region constructor

        private readonly IDiscountService _discountService;
        private readonly ISellerService _sellerService;
        private readonly IProductService _productService;

        public ProductDiscountController(IDiscountService discountService, ISellerService sellerService, IProductService productService)
        {
            _discountService = discountService;
            _sellerService = sellerService;
            _productService = productService;
        }

        #endregion

        #region filter discount

        [HttpGet("discounts")]
        [HttpGet("discounts/{ProductId}")]
        public async Task<IActionResult> FilterDiscounts(FilterProductDiscountDTO filter)
        {
            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());

            if (seller == null)
            {
                return NotFound();
            }

            filter.SellerId = seller.Id;

            var result = await _discountService.FilterProductDiscount(filter);

            return View(result);
        }

        #endregion

        #region create discount

        [HttpGet("create-discount")]
        public async Task<IActionResult> CreateDiscount()
        {
            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
            var products = await _productService.FilterProductsForSellerByProductName(seller!.Id);

            ViewData["products"] = new SelectList(products, "Value", "Text");

            return View();
        }

        [HttpPost("create-discount"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDiscount(CreateProductDiscountDto discount)
        {
            if (ModelState.IsValid)
            {
                var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());
                var result = await _discountService.CreateProductDiscount(discount, seller.Id);

                switch (result)
                {
                    case CreateDiscountResult.Error:
                        TempData[ErrorMessage] = "عملیات ثبت تخفیف مورد نظر با شکست مواجه شد";
                        break;
                    case CreateDiscountResult.ProductNotFound:
                        TempData[WarningMessage] = "محصول مورد نظر یافت نشد";
                        break;
                    case CreateDiscountResult.ProductIsNotForSeller:
                        TempData[WarningMessage] = "محصول مورد نظر یافت نشد";
                        break;
                    case CreateDiscountResult.Success:
                        TempData[SuccessMessage] = "عملیات ثبت تخفیف برای محصول مورد نظر با موفقیت انجام شد";
                        return RedirectToAction("FilterDiscounts");
                }
            }
            return View(discount);
        }

        #endregion
    }
}
