using Junko.DataLayer.Context;
using Junko.Domain.Entities.Account;
using Junko.Domain.InterFaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Junko.DataLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Constructor

        private JunkoDbContext _context;

        public UserRepository(JunkoDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        public async Task<bool> IsUserExistsByMobileNumber(string mobile)
        {
            return await _context.Users.Where(u => !u.IsDelete).AnyAsync(s => s.Mobile.Equals(mobile));
        }

        public async Task<IQueryable<User>> GetUserQuery()
        {
            return _context.Users.Where(u => !u.IsDelete).AsQueryable();
        }

        public async Task AddUser(User user)
        {
           await _context.Users.AddAsync(user);
        }

        public async Task AddRangeUsers(List<User> users)
        {
            foreach (var user in users)
            {
                await AddUser(user);
            }
        }

        public async Task<User?> GetUserById(long userId)
        {
            return await _context.Users.SingleOrDefaultAsync(e => e.Id.Equals(userId));
        }

        public async Task<User?> GetUserByMobile(string mobile)
        {
            return await _context.Users.SingleOrDefaultAsync(e => e.Mobile == mobile);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(e => e.Email == email);
        }

        public async Task<User?> GetUserByActiveCode(string activeCode)
        {
            return await _context.Users.SingleOrDefaultAsync(c => c.EmailActiveCode == activeCode);
        }

        public void UpdateUser(User user)
        {
            user.LastUpdateDate = DateTime.Now;
            _context.Users.Update(user);
        }

        public void DeleteUser(User user)
        {
            user.IsDelete = true;

            UpdateUser(user);
        }

        public async Task DeleteUser(long userId)
        {
            var user = await GetUserById(userId);

            if (user != null)
            {
                DeleteUser(user);
            }
        }

        public void DeleteUserPermanent(User user)
        {
            _context.Users.Remove(user);
        }

        public void DeleteUserPermanentUsers(List<User> users)
        {
            _context.Users.RemoveRange(users);
        }

        public async Task DeleteUserPermanent(long userId)
        {
            var user = await GetUserById(userId);

            if (user != null)
            {
                DeleteUserPermanent(user);
            }
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
