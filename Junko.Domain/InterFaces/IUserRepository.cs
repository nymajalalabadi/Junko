using Junko.Domain.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junko.Domain.InterFaces
{
    public interface IUserRepository
    {
        #region Methods

        Task<bool> IsUserExistsByMobileNumber(string mobile);

        Task<IQueryable<User>> GetUserQuery();

        Task AddUser(User user);

        Task AddRangeUsers(List<User> users);

        Task<User?> GetUserById(long userId);

        Task<User?> GetUserByMobile(string mobile);

        Task<User?> GetUserByEmail(string email);

        Task<User?> GetUserByActiveCode(string activeCode);

        void UpdateUser(User user);

        void DeleteUser(User user);

        Task DeleteUser(long userId);

        void DeleteUserPermanent(User user);

        void DeleteUserPermanentUsers(List<User> users);

        Task DeleteUserPermanent(long userId);

        Task SaveChanges();

        #endregion
    }
}
