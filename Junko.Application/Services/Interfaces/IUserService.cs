﻿using Junko.Domain.Entities.Account;
using Junko.Domain.ViewModels.Account;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Application.Services.Interfaces
{
    public interface IUserService
    {
        #region Methods

        Task<RegisterUserResult> RegisterUser(RegisterUserDTO register);

        Task<bool> IsUserExistsByMobileNumber(string mobile);

        Task<LoginUserResult> GetUserForLogin(LoginUserDTO login);

        Task<User?> GetUserByMobile(string mobile);

        Task<User?> GetUserByEmail(string email);

        Task<ForgotPasswordResult> RecoverUserPassword(ForgotPasswordDTO forgot);

        Task<bool> ActiveAccount(string activeCode);

        Task<bool> ResetPassword(ResetPasswordDTO reset);

        Task<bool> ChangeUserPassword(ChangePasswordDTO changePass, long currentUserId);

        Task<EditUserProfileDTO> GetProfileForEdit(long userId);

        Task<EditUserProfileResult> EditUserProfile(EditUserProfileDTO profile, long userId, IFormFile avatarImage);

        #endregion
    }
}
