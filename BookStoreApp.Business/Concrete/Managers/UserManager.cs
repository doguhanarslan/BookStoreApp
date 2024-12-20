using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Business.Abstract;
using BookStoreApp.Core.CrossCuttingConcerns.Caching;
using BookStoreApp.DataAccess.Abstract;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.Business.Concrete.Managers
{
    public class UserManager: IUserService
    {
        private IUserDal _userDal;
        private ICacheService _cacheService;
        public UserManager(IUserDal userDal, ICacheService cacheService)
        {
            _userDal = userDal;
            _cacheService = cacheService;
        }


        public User GetById(int id)
        {
            return _userDal.Get(u => u.Id == id);
        }

        //public User? GetUser(string userName, string password)
        //{
        //    return GetUserFromCache(userName, password);
        //}

        //public User? GetUserFromCache(string userName, string password)
        //{
        //    return _cacheService.GetOrAddUser(userName, password,()=>_userDal.GetUser(userName,password));
        //}
        public async Task<User> ValidateUserAsync(string username, string password)
        {
            var user = _userDal.GetUser(username, password);
            if (user == null || user.Password != password)
            {
                return null;
            }

            return user;
        }
        private string HashPassword(string password)
        {
            // Şifreyi hashleme işlemi (örneğin, SHA256)
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
