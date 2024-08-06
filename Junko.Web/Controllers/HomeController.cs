using GoogleReCaptcha.V3.Interface;
using Junko.Application.Services.Interfaces;
using Junko.Domain.ViewModels.ContactUs;
using Junko.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
using Junko.Application.Extensions;

namespace Junko.Web.Controllers
{
    public class HomeController : SiteBaseController
    {
        #region constructor

        private readonly IContactService _contactService;
        private readonly ICaptchaValidator _captchaValidator;

        public HomeController(IContactService contactService, ICaptchaValidator captchaValidator)
        {
            _contactService = contactService;
            _captchaValidator = captchaValidator;
        }

        #endregion

        #region index

        public async Task<IActionResult> Index()
        {
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
                TempData[ErrorMessage] = "?? ????? ??? ????? ???";
                return View(contact);
            }

            if (ModelState.IsValid)
            {
                var ip = HttpContext.GetUserIp();

                await _contactService.CreateContactUs(contact, HttpContext.GetUserIp(), User.GetUserId());

                TempData[SuccessMessage] = "???? ??? ?? ?????? ????? ??";
                return RedirectToAction("ContactUs");
            }

            return View(contact);
        }

        #endregion

    }
}
