using Junko.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Junko.Web.ViewComponents
{
    #region site header

    public class SiteHeaderViewComponent : ViewComponent
    {
        #region consractor

        private readonly ISiteSettingService _settingService;

        public SiteHeaderViewComponent(ISiteSettingService settingService)
        {
            _settingService = settingService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.siteSetting = await _settingService.GetDefaultSiteSetting();

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
}
