using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RestAPIDemo.Extensions;
using RestAPIDemo.Models;
using RestAPIDemo.Models.Extra;

namespace RestAPIDemo.Services
{
    public class UserSqlService : IUserService
    {

        // DB context to connect with Sql Server
        private ApplicationDBContext _userContext;
        public UserSqlService(ApplicationDBContext usercontext)
        {
            _userContext = usercontext;
        }

        // Create New User 
        public User AddUser(UserDto nwuser)
        {
            User user = new User();
            user.UserId = nwuser.UserId;
            user.Name = nwuser.Name;
            user.Country = nwuser.FullAddress.Country;
            user.FullAddress = UserAddressFormatter.FormatUserAddress(nwuser.FullAddress);
            user.JoiningDate = nwuser.JoiningDate;
            user.ImagePath = nwuser.ImagePath;
            _userContext.Users.Add(user);
            _userContext.SaveChanges();
            return user;

        }

        // Delete User from Database
        public void DeletUser(User user)
        {
            // remove this user from the list and then commit
            _userContext.Users.Remove(user);
            _userContext.SaveChanges();
        }


        // get user details from Database
        public User GetUser(Guid UserID)
        {
            User user = _userContext.Users.Find(UserID);
            return user;
        }

        // get list of users based on parammeters passed
        public List<User> GetUsers(string sortOrder, string searchString, int? pageNumber)
        {
            // initial query 
            var usersList = from s in _userContext.Users
                           select s;

            // if there is a search string included in the request
            if (!String.IsNullOrEmpty(searchString))
            {
                usersList = usersList.Where(s => s.Name.Contains(searchString));
            }

            // switch between type of sorting: sort by name, by country, and joining Date 
            switch (sortOrder)
            {
                case "name":
                    usersList = usersList.OrderByDescending(s => s.Name);
                    break;
                case "country":
                    usersList = usersList.OrderByDescending(s => s.Country);
                    break;
                case "joiningDate":
                    usersList = usersList.OrderByDescending(s => s.JoiningDate);
                    break;
                default:
                    usersList = usersList.OrderByDescending(s => s.UserId);
                    break;
            }

            // if the page number is not defined, ignore pagination and return all data
            if (pageNumber == null)
            {
                return usersList.ToList();
            }
            else
            {
                int pageSize = 2; // define number of items in each page
                var count = _userContext.Users.Count();
                return usersList.Skip((int)((pageNumber - 1) * pageSize)).Take(pageSize).ToList();
            }
            
        }

        // Update User details and commit it to database
        public User UpdateUser(UserDto user)
        {
            User ExistingUser = GetUser(user.UserId);
            ExistingUser.UserId = user.UserId;
            ExistingUser.Name = user.Name;
            ExistingUser.Country = user.FullAddress.Country;
            ExistingUser.FullAddress = UserAddressFormatter.FormatUserAddress(user.FullAddress);;
            ExistingUser.JoiningDate = user.JoiningDate;
            ExistingUser.Designation = user.Designation;
            ExistingUser.ImagePath = user.ImagePath;
            _userContext.SaveChanges();
            return ExistingUser;
        }

        // Update Only User Profile Image
        public User UpdateUserProfile(Guid UserID, string ImagePath)
        {
            User user = GetUser(UserID);
            user.ImagePath = ImagePath;
            _userContext.SaveChanges();
            return user;
        }
    }
}
