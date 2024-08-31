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
            filter.TakeEntity = 2;
            var products = await _productService.FilterProducts(filter);

            ViewBag.ProductCategories = await _productService.GetAllActiveProductCategories();

            if (filter.CurrentPage > filter.GetLastPage() && filter.GetLastPage() != 0)
            {
                return NotFound();
            }

            return View(products);
        }

        #endregion

        #region show product detail

        [HttpGet("products/{productId}/{title}")]
        public async Task<IActionResult> ProductDetail(long productId, string title)
        {
            var product = await _productService.GetProductDetailById(productId);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        #endregion


    }
}
