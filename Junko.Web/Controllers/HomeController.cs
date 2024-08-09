using GoogleReCaptcha.V3.Interface;
using Junko.Application.Services.Interfaces;
using Junko.Domain.ViewModels.ContactUs;
using Junko.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
using Junko.Application.Extensions;
using Junko.Application.Services.Implementations;
using Junko.Domain.Entities.Site;

namespace Junko.Web.Controllers
{
    public class HomeController : SiteBaseController
    {
        #region constructor

        private readonly IContactService _contactService;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly ISiteSettingService _siteSettingService;

        public HomeController(IContactService contactService, ICaptchaValidator captchaValidator, ISiteSettingService siteSettingService)
        {
            _contactService = contactService;
            _captchaValidator = captchaValidator;
            _siteSettingService = siteSettingService;
        }

        #endregion

        #region index

        public async Task<IActionResult> Index()
        {
            ViewBag.banners = await _siteSettingService
                .GetSiteBannersByPlacement(new List<BannerPlacement>
                {
                    BannerPlacement.Home_1,
                    BannerPlacement.Home_2,
                    BannerPlacement.Home_3
                });

            return View();
        }

        #endregion

        #region contact us

        [HttpGet("contact-us")]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost("contact-us"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(CreateContactUsDTO contact)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(contact.Captcha))
            {
                TempData[ErrorMessage] = "پیام شما ارسال نشد";
                return View(contact);
            }

            if (ModelState.IsValid)
            {
                var ip = HttpContext.GetUserIp();

                await _contactService.CreateContactUs(contact, HttpContext.GetUserIp(), User.GetUserId());

                TempData[SuccessMessage] = "پیام شما ارسال شد";
                return RedirectToAction("ContactUs");
            }

            return View(contact);
        }

        #endregion

        #region about us

        [HttpGet("about-us")]
        public async Task<IActionResult> AboutUs()
        {
            var siteSetting = await _siteSettingService.GetDefaultSiteSetting();

            return View(siteSetting);
        }

        #endregion


    }
}
