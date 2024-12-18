using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Business.Abstract;
using BookStoreApp.DataAccess.Abstract;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.Business.Concrete.Managers
{
    public class UserManager: IUserService
    {
        private IUserDal _userDal;
        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }


        public User GetById(int Id)
        {
            return _userDal.Get(u => u.Id == Id);
        }

        public User? GetUser(string userName, string password)
        {
            return _userDal.GetUser(userName,password);
        }
    }
}
