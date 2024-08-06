using Junko.Domain.Entities.Account;
using Junko.Domain.ViewModels.Account;
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

        Task<User> GetUserByMobile(string mobile);

        Task<ForgotPasswordResult> RecoverUserPassword(ForgotPasswordDTO forgot);

        Task<bool> ActiveAccount(string activeCode);

        Task<bool> ResetPassword(ResetPasswordDTO reset);

        #endregion
    }
}
