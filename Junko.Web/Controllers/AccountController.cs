using GoogleReCaptcha.V3.Interface;
using Junko.Application.Services.Interfaces;
using Junko.Domain.ViewModels.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Junko.Application.Security;
using Junko.Domain.Entities.Account;
using Microsoft.SqlServer.Server;

namespace Junko.Web.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region constructor

        private readonly IUserService _userService;

        private readonly ICaptchaValidator _captchaValidator;

        public AccountController(IUserService userService, ICaptchaValidator captchaValidator)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
        }

        #endregion

        #region register

        [HttpGet("register")]
        public IActionResult Register()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return Redirect("/");
            }

            return View();
        }

        [HttpPost("register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserDTO register)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(register.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(register);
            }

            if (ModelState.IsValid)
            {
                var res = await _userService.RegisterUser(register);
                switch (res)
                {
                    case RegisterUserResult.MobileExists:
                        TempData[ErrorMessage] = "تلفن همراه وارد شده تکراری می باشد";
                        ModelState.AddModelError("Mobile", "تلفن همراه وارد شده تکراری می باشد");
                        break;

                    case RegisterUserResult.Success:
                        TempData[SuccessMessage] = "ثبت نام شما با موفقیت انجام شد";
                        TempData[InfoMessage] = "کد تایید تلفن همراه برای شما ارسال شد";
                        return RedirectToAction("Login");
                }
            }

            return View(register);
        }

        #endregion

        #region login

        [HttpGet("login")]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return Redirect("/");
            }

            return View();
        }

        [HttpPost("login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserDTO login)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(login.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(login);
            }

            if (ModelState.IsValid)
            {
                var result = await _userService.GetUserForLogin(login);

                switch (result)
                {
                    case LoginUserResult.NotFound:
                        TempData[ErrorMessage] = "کاربر مورد نظر یافت نشد";
                        break;

                    case LoginUserResult.NotActivated:
                        TempData[WarningMessage] = "حساب کاربری شما فعال نشده است";
                        break;

                    case LoginUserResult.Success:

                        var user = await _userService.GetUserByEmail(login.Email);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user!.Email),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                        };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var properties = new AuthenticationProperties
                        {
                            IsPersistent = login.RememberMe
                        };

                        await HttpContext.SignInAsync(principal, properties);

                        TempData[SuccessMessage] = "عملیات ورود با موفقیت انجام شد";
                        return Redirect("/");
                }
            }

            return View(login);
        }

        #endregion

        #region activate Email

        [HttpGet("ActiveAccount/{EmailActiveCode}")]
        public async Task<IActionResult> ActiveAccount(string EmailActiveCode)
        {
            ViewBag.IsActive = await _userService.ActiveAccount(EmailActiveCode);

            return View();
        }

        #endregion


        #region forgot password

        [HttpGet("forgot-pass")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("forgot-pass"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgot)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(forgot.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(forgot);
            }

            if (ModelState.IsValid)
            {
                var result = await _userService.RecoverUserPassword(forgot);

                switch (result)
                {
                    case ForgotPasswordResult.NotFound:
                        TempData[WarningMessage] = "کاربر مورد نظر یافت نشد";
                        break;

                    case ForgotPasswordResult.Success:
                        TempData[SuccessMessage] = "کلمه ی عبور جدید برای شما ارسال شد";
                        TempData[InfoMessage] = "لطفا پس از ورود به حساب کاربری ، کلمه ی عبور خود را تغییر دهید";
                        return RedirectToAction("Login");
                }
            }

            return View(forgot);
        }

        #endregion

        #region Reset Password

        [HttpGet("ResetPassword/{EmailActiveCode}")]
        public ActionResult ResetPassword(string EmailActiveCode)
        {
            var model = new ResetPasswordDTO() 
            {
                ActiveCode = EmailActiveCode
            };

            return View(model);
        }

        [HttpPost("ResetPassword/{EmailActiveCode}")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO reset)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(reset.Captcha))
            {
                TempData[ErrorMessage] = "کد کپچای شما تایید نشد";
                return View(reset);
            }

            if (!ModelState.IsValid)
            {
                return View(reset);

            }

            var result = await _userService.ResetPassword(reset);

            if (result == true)
            {
                TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
            }
            else
            {
                TempData[ErrorMessage] = "عملیات با شکست مواجه شد";
            }

            return Redirect("/Login");
        }
        #endregion

        #region log out

        [HttpGet("log-out")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();

            return Redirect("/");
        }

        #endregion
    }
}
