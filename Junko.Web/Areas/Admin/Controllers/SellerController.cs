using Junko.Application.Services.Interfaces;
using Junko.Domain.ViewModels.Store;
using Junko.Web.Http;
using Microsoft.AspNetCore.Mvc;

namespace Junko.Web.Areas.Admin.Controllers
{
    public class SellerController : AdminBaseController
    {
        #region constructor

        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        #endregion

        #region seller requests

        public async Task<IActionResult> SellerRequests(FilterSellerDTO filter)
        {
            var model = await _sellerService.FilterSellers(filter);

            return View(model);
        }

        #endregion

        #region accept seller request

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptSellerRequest(long requestId)
        {
            var result = await _sellerService.AcceptSellerRequest(requestId);

            if (result)
            {
                return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, "درخواست مورد نظر با موفقیت تایید شد", null);
            }

            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger, "اطلاعاتی با این مشخصات یافت نشد", null);
        }

        #endregion

        #region reject seller request

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectSellerRequest(RejectItemDTO reject)
        {
            var result = await _sellerService.RejectSellerRequest(reject);

            if (result)
            {
                return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, "درخواست مورد نظر با موفقیت رد شد شد", reject);
            }

            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger, "اطلاعاتی با این مشخصات یافت نشد", null);
        }

        #endregion

    }
}
