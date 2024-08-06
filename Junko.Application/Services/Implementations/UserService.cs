using Junko.Application.Security;
using Junko.Application.Services.Interfaces;
using Junko.Domain.Entities.Account;
using Junko.Domain.InterFaces;
using Junko.Domain.ViewModels.Account;

namespace Junko.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        #region Constructor

        private readonly IUserRepository _userRepository;
        private readonly IViewRenderService _viewRenderService;
        private readonly IEmailService _emailService;

        public UserService(IUserRepository userRepository, IViewRenderService viewRenderService, IEmailService emailService)
        {
            _userRepository = userRepository;
            _viewRenderService = viewRenderService;
            _emailService = emailService;
        }

        #endregion

        #region Methods

        public async Task<RegisterUserResult> RegisterUser(RegisterUserDTO register)
        {
            if (!await _userRepository.IsUserExistsByMobileNumber(register.Mobile))
            {
                var user = new User
                {
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Mobile = register.Mobile,
                    Password = PasswordHelper.EncodePasswordMd5(register.Password),
                    MobileActiveCode = new Random().Next(10000, 999999).ToString(),
                    EmailActiveCode = Guid.NewGuid().ToString("N")
                };

                await _userRepository.AddUser(user);
                await _userRepository.SaveChanges();


                #region Active Email

                string body = await _viewRenderService.RenderToStringAsync("_ActiveEmail", user);

                _emailService.SendEmail(user.Email, "فعالسازی", body);

                #endregion

                return RegisterUserResult.Success;
            }

            return RegisterUserResult.MobileExists;
        }

        public async Task<bool> IsUserExistsByMobileNumber(string mobile)
        {
            return await _userRepository.IsUserExistsByMobileNumber(mobile);
        }

        public async Task<LoginUserResult> GetUserForLogin(LoginUserDTO login)
        {
            var user = await _userRepository.GetUserByMobile(login.Mobile);

            if (user == null)
            {
                return LoginUserResult.NotFound;
            }

            if (!user.IsMobileActive)
            {
                return LoginUserResult.NotActivated;
            }

            if (user.Password != PasswordHelper.EncodePasswordMd5(login.Password))
            {
                return LoginUserResult.NotFound;
            }

            return LoginUserResult.Success;
        }

        public async Task<User> GetUserByMobile(string mobile)
        {
            var user = await _userRepository.GetUserByMobile(mobile);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<ForgotPasswordResult> RecoverUserPassword(ForgotPasswordDTO forgot)
        {
            var user = await _userRepository.GetUserByEmail(forgot.Email);

            if (user == null)
            {
                return ForgotPasswordResult.NotFound;
            }

            #region MyRegion

            string BodyEmail = await _viewRenderService.RenderToStringAsync("_ForgotPassword", user);
            _emailService.SendEmail(user.Email, "بازیابی کلمه عبور", BodyEmail);

            #endregion

            await _userRepository.SaveChanges();

            return ForgotPasswordResult.Success;
        }

        public async Task<bool> ActiveAccount(string activeCode)
        {
            var user = await _userRepository.GetUserByActiveCode(activeCode);

            if (user == null || user.IsEmailActive)
            {
                return false;
            }

            user.IsEmailActive = true;

            user.EmailActiveCode = new Random().Next(1000000, 999999999).ToString();

            _userRepository.UpdateUser(user);
            await _userRepository.SaveChanges();

            return true;
        }

        public async Task<bool> ResetPassword(ResetPasswordDTO reset)
        {
            var user = await _userRepository.GetUserByActiveCode(reset.ActiveCode);

            if (user == null)
            {
                return false;
            }

            string hashNewPassword = PasswordHelper.EncodePasswordMd5(reset.Password);
            user.Password = hashNewPassword;

            user.EmailActiveCode = new Random().Next(1000000, 999999999).ToString();

            _userRepository.UpdateUser(user);
            await _userRepository.SaveChanges();

            return true;
        }

        #endregion
    }
}
