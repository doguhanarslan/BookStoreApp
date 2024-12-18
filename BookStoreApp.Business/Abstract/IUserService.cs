using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.Business.Abstract
{
    public interface IUserService
    {
        User? GetUser(string userName,string password);
        User? GetUserFromCache(string userName, string password);
    }
}
