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

                    case RequestSellerResult.Success:
                        TempData[SuccessMessage] = "درخواست شما با موفقیت ثبت شد";
                        TempData[InfoMessage] = "فرایند تایید اطلاعات شما در حال پیگیری می باشد";
                        return RedirectToAction("SellerRequests");
                }

            }

            return View(requestSeller);
        }
        #endregion
    }
}
