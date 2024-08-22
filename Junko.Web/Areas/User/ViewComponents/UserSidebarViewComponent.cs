using Junko.Application.Extensions;
using Junko.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Junko.Web.Areas.User.ViewComponents
{
    public class UserSidebarViewComponent : ViewComponent
    {
        #region Consractor

        private ISellerService _sellerService;

        public UserSidebarViewComponent(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.hasUserAnyActiveSellerPanel = await _sellerService.HasUserAnyActiveSellerPanel(User.GetUserId());

            return View("UserSidebar");
        }
    }

}
