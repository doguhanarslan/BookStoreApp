using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Core.DataAccess;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.DataAccess.Abstract
{
    public interface IUserDal: IEntityRepository<User>
    {
        User? GetById(int id);

        User? GetUser(string userName,string password);

        User? GetUserByUserName(string userName);

        User? AddUser(User user);
    }
}
