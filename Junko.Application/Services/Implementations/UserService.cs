using Junko.Application.Generators;
using Junko.Application.Security;
using Junko.Application.Services.Interfaces;
using Junko.Application.Statics;
using Junko.Domain.Entities.Account;
using Junko.Domain.InterFaces;
using Junko.Domain.ViewModels.Account;
using Microsoft.AspNetCore.Http;
using System.Reflection;

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
            if (!await _userRepository.IsUserExistsByEmail(register.Email))
            {
                var user = new User
                {
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Email = register.Email,
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
            var user = await _userRepository.GetUserByEmail(login.Email);

            if (user == null)
            {
                return LoginUserResult.NotFound;
            }

            if (!user.IsEmailActive)
            {
                return LoginUserResult.NotActivated;
            }

            if (user.Password != PasswordHelper.EncodePasswordMd5(login.Password))
            {
                return LoginUserResult.NotFound;
            }

            return LoginUserResult.Success;
        }

        public async Task<User?> GetUserByMobile(string mobile)
        {
            var user = await _userRepository.GetUserByMobile(mobile);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);

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

            user.EmailActiveCode = Guid.NewGuid().ToString("N");

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

            user.EmailActiveCode = Guid.NewGuid().ToString("N");

            _userRepository.UpdateUser(user);
            await _userRepository.SaveChanges();

            return true;
        }

        public async Task<bool> ChangeUserPassword(ChangePasswordDTO changePass, long currentUserId)
        {
            var user = await _userRepository.GetUserById(currentUserId);

            if (user != null)
            {
                var newPassword = PasswordHelper.EncodePasswordMd5(changePass.NewPassword);

                if (newPassword != user.Password)
                {
                    user.Password = newPassword;

                    _userRepository.UpdateUser(user);
                    await _userRepository.SaveChanges();

                    return true;
                }
            }

            return false;

        }

        public async Task<EditUserProfileDTO> GetProfileForEdit(long userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                return null;
            }

            return new EditUserProfileDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Avatar = user.Avatar
            };

        }

        public async Task<EditUserProfileResult> EditUserProfile(EditUserProfileDTO profile, long userId, IFormFile avatarImage)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null) return EditUserProfileResult.NotFound;

            if (user.IsBlocked)
            {
                return EditUserProfileResult.IsBlocked;
            }

            if (!user.IsMobileActive)
            {
                return EditUserProfileResult.IsNotActive;
            }


            if (avatarImage != null && avatarImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(avatarImage.FileName);
                avatarImage.AddImageToServer(imageName, SiteTools.UserAvatarOrigin, 100, 100, SiteTools.UserAvatarThumb, user.Avatar);
                user.Avatar = imageName;
            }

            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;

            _userRepository.UpdateUser(user);
            await _userRepository.SaveChanges();

            return EditUserProfileResult.Success;
        }

        #endregion
    }
}
