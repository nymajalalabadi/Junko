using Junko.Application.Extensions;
using Junko.Application.Services.Interfaces;
using Junko.Domain.ViewModels.Store;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace Junko.Web.Areas.User.Controllers
{
    public class SellerController : UserBaseController
    {
        #region constructor

        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        #endregion

        #region Actions

        #region seller requests

        [HttpGet("seller-requests")]
        public async Task<IActionResult> SellerRequests(FilterSellerDTO filter)
        {
            filter.UserId = User.GetUserId();
            filter.State = FilterSellerState.All;
            filter.TakeEntity = 1;
            var model = await _sellerService.FilterSellers(filter);

            return View(model);
        }

        #endregion

        #region request seller

        [HttpGet("request-seller-panel")]
        public IActionResult RequestSellerPanel()
        {
            return View();
        }

        [HttpPost("request-seller-panel"), ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestSellerPanel(RequestSellerDTO requestSeller)
        {
            if (ModelState.IsValid)
            {
                var result = await _sellerService.AddNewSellerRequest(requestSeller, User.GetUserId());

                switch (result)
                {
                    case RequestSellerResult.HasNotPermission:
                        TempData[ErrorMessage] = "شما دسترسی لازم جهت انجام فرایند مورد نظر را ندارید";
                        break;

                    case RequestSellerResult.HasUnderProgressRequest:
                        TempData[WarningMessage] = "درخواست های قبلی شما در حال پیگیری می باشند";
                        TempData[InfoMessage] = "در حال حاضر نمیتوانید درخواست جدیدی ثبت کنید";
                        break;

                    case RequestSellerResult.HasSeller:
                        TempData[WarningMessage] = "برای شما از قبل یک فروشگاه ثبت شده است";
                        TempData[InfoMessage] = "در حال حاضر نمیتوانید درخواست جدیدی ثبت کنید";
                        break;

                    case RequestSellerResult.Success:
                        TempData[SuccessMessage] = "درخواست شما با موفقیت ثبت شد";
                        TempData[InfoMessage] = "فرایند تایید اطلاعات شما در حال پیگیری می باشد";
                        return RedirectToAction("SellerRequests");
                }

            }

            return View(requestSeller);
        }

        #endregion

        #region edit request

        [HttpGet("edit-request-seller/{id}")]
        public async Task<IActionResult> EditRequestSeller(long id)
        {
            var requestSeller = await _sellerService.GetRequestSellerForEdit(id, User.GetUserId());

            if (requestSeller == null)
            {
                return NotFound();
            }

            return View(requestSeller);
        }

        [HttpPost("edit-request-seller/{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRequestSeller(EditRequestSellerDTO request)
        {
            if (ModelState.IsValid)
            {
                var result = await _sellerService.EditRequestSeller(request, User.GetUserId());

                switch (result)
                {
                    case EditRequestSellerResult.NotFound:
                        TempData[ErrorMessage] = "اطلاعات مورد نظر یافت نشد";
                        break;

                    case EditRequestSellerResult.Success:
                        TempData[SuccessMessage] = "اطلاعات مورد نظر با موفقیت ویرایش شد";
                        TempData[InfoMessage] = "فرآیند تایید اطلاعات از سر گرفته شد";
                        return RedirectToAction("SellerRequests");
                }
            }

            return View(request);
        }

        #endregion


        #endregion
    }
}
