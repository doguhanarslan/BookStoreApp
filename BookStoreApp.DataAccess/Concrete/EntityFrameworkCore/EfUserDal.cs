using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Core.DataAccess.EntityFrameworkCore;
using BookStoreApp.DataAccess.Abstract;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.DataAccess.Concrete.EntityFrameworkCore
{
    public class EfUserDal : EfEntityRepositoryBase<User, BookstoreContext>, IUserDal
    {
        private BookstoreContext _context;

        public EfUserDal(BookstoreContext context)
        {
            _context = context;
        }

        public User? GetUser(string userName,string password)
        {
            var result = from u in _context.Users
                         join ur in _context.UserRoles on u.Id equals ur.UserId
                         join r in _context.Roles on ur.RoleId equals r.Id
                         where u.UserName == userName
                            && u.Password == password
                         select new User
                         {
                             Id = u.Id,
                             FirstName = u.FirstName,
                             LastName = u.LastName,
                             Email = u.Email,
                             UserName = u.UserName,
                             Password = u.Password,
                             ProfileImage = u.ProfileImage,
                         };
            return result.SingleOrDefault();
        }

        public User? GetById(int id)
        {
            var result = from u in _context.Users
                         join ur in _context.UserRoles on u.Id equals ur.UserId
                         join r in _context.Roles on ur.RoleId equals r.Id
                         where u.Id == id
                         select new User
                         {
                             Id = u.Id,
                             FirstName = u.FirstName,
                             LastName = u.LastName,
                             Email = u.Email,
                             UserName = u.UserName,
                             Password = u.Password,
                             ProfileImage = u.ProfileImage,
                         };
            return result.FirstOrDefault();
        }
    }
}
