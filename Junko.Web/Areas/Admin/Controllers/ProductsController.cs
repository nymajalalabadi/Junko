using Junko.Application.Services.Implementations;
using Junko.Application.Services.Interfaces;
using Junko.Domain.ViewModels.Products;
using Junko.Domain.ViewModels.Store;
using Junko.Web.Http;
using Microsoft.AspNetCore.Mvc;

namespace Junko.Web.Areas.Admin.Controllers
{
    public class ProductsController : AdminBaseController
    {
        #region constructor

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region filter products

        public async Task<IActionResult> Index(FilterProductDTO filter)
        {
            var result = await _productService.FilterProducts(filter);

            return View(result);
        }

        #endregion

        #region accept product

        public async Task<IActionResult> AcceptSellerProduct(long id)
        {
            var result = await _productService.AcceptSellerProduct(id);

            if (result)
            {
                return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, "محصول مورد نظر با موفقیت تایید شد", null);
            }

            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger, "محصول مورد نظر یافت نشد", null);
        }

        #endregion

        #region reject Product request

        [HttpGet]
        public async Task<IActionResult> RejectProduct(long id)
        {
            var model = new RejectItemDTO()
            {
                Id = id
            };

            return PartialView("_RejectProductPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> RejectProductRequest(RejectItemDTO reject)
        {
            var result = await _productService.RejectProductRequest(reject);

            if (result)
            {
                return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, "درخواست مورد نظر با موفقیت رد شد", reject);
            }

            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger, "اطلاعاتی با این مشخصات یافت نشد", null);
        }

        #endregion
    }
}
