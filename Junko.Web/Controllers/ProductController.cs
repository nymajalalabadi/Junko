using Junko.Application.Services.Interfaces;
using Junko.Domain.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;

namespace Junko.Web.Controllers
{
    public class ProductController : SiteBaseController
    {
        #region constructor

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region filter products

        [HttpGet("products")]
        public async Task<IActionResult> FilterProducts(FilterProductDTO filter)
        {
            var products = await _productService.FilterProducts(filter);

            ViewBag.ProductCategories = await _productService.GetAllActiveProductCategories();

            return View(products);
        }

        #endregion

    }
}
