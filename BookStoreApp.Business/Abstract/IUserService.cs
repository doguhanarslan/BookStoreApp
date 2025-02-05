using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStoreApp.Business.DTOs;
using BookStoreApp.Entities.Concrete;

namespace BookStoreApp.Business.Abstract
{
    public interface IUserService
    {
        //User? GetUser(string userName,string password);
        //User? GetUserFromCache(string userName, string password);
        Task<User> ValidateUserAsync(string username, string password);
        Task<User> AddUserAsync(User user);
    }
}
