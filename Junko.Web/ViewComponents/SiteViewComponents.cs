using Junko.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Junko.Web.ViewComponents
{
    #region site header

    public class SiteHeaderViewComponent : ViewComponent
    {
        #region consractor

        private readonly ISiteSettingService _settingService;
        private readonly IUserService _userService;

        public SiteHeaderViewComponent(ISiteSettingService settingService, IUserService userService)
        {
            _settingService = settingService;
            _userService = userService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.siteSetting = await _settingService.GetDefaultSiteSetting();

            ViewBag.user = await _userService.GetUserByEmail(User.Identity.Name);

            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = await _userService.GetUserByEmail(User.Identity.Name);
            }

            return View("SiteHeader");
        }
    }

    #endregion

    #region site footer

    public class SiteFooterViewComponent : ViewComponent
    {
        #region consractor

        private readonly ISiteSettingService _settingService;

        public SiteFooterViewComponent(ISiteSettingService settingService)
        {
            _settingService = settingService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.siteSetting = await _settingService.GetDefaultSiteSetting();

            return View("SiteFooter");
        }
    }

    #endregion

    #region home sliders

    public class HomeSliderViewComponent : ViewComponent
    {
        #region consractor

        private readonly ISiteSettingService _settingService;

        public HomeSliderViewComponent(ISiteSettingService settingService)
        {
            _settingService = settingService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliders = await _settingService.GetAllActiveSliders();

            return View("HomeSlider", sliders);
        }
    }

    #endregion

}
