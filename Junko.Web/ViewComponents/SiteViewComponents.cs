using Junko.Application.Extensions;
using Junko.Application.Services.Implementations;
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
        private readonly IProductService _productService;

        public SiteHeaderViewComponent(ISiteSettingService settingService, IUserService userService, IProductService productService)
        {
            _settingService = settingService;
            _userService = userService;
            _productService = productService;
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

            ViewBag.ProductCategories = await _productService.GetAllActiveProductCategories();

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

    #region user order

    public class UserOrderViewComponent : ViewComponent
    {
        private readonly IOrderService _orderService;

        public UserOrderViewComponent(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var openOrder = await _orderService.GetUserOpenOrderDetail(User.GetUserId());

            return View("UserOrder", openOrder);
        }
    }

    #endregion


}
