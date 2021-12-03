using System;
using System.Collections.Generic;
using RestAPIDemo.Models;
using RestAPIDemo.Models.Extra;

namespace RestAPIDemo.Services
{
    public interface IUserService
    {
        List<User> GetUsers(string sortOrder, string searchString, int? pageNumber);
        User GetUser(Guid UserID);
        User AddUser(UserDto user);
        User UpdateUser(UserDto user);
        User UpdateUserProfile(Guid UserID, string ImagePath);
        void DeletUser(User user);
    }
}
