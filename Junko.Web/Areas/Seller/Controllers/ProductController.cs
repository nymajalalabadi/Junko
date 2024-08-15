using Junko.Application.Extensions;
using Junko.Application.Services.Interfaces;
using Junko.Domain.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;

namespace Junko.Web.Areas.Seller.Controllers
{
    public class ProductController : SellerBaseController
    {
        #region constructor

        private readonly IProductService _productService;

        private readonly ISellerService _sellerService;

        public ProductController(IProductService productService, ISellerService sellerService)
        {
            _productService = productService;
            _sellerService = sellerService;
        }

        #endregion

        #region list

        [HttpGet("products")]
        public async Task<IActionResult> Index(FilterProductDTO filter)
        {
            var seller = await _sellerService.GetLastActiveSellerByUserId(User.GetUserId());

            filter.SellerId = seller.Id;

            filter.FilterProductState = FilterProductState.Active;

            var model = await _productService.FilterProducts(filter);

            return View(model);
        }

        #endregion

        #region create product

        [HttpGet("create-product")]
        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.MainCategories = await _productService.GetAllProductCategoriesByParentId(null);

            return View();
        }

        [HttpPost("create-product"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductDTO product)
        {
            if (ModelState.IsValid)
            {
                // todo: create product
            }

            ViewBag.MainCategories = await _productService.GetAllProductCategoriesByParentId(null);
            return View(product);
        }

        #endregion

    }

}
