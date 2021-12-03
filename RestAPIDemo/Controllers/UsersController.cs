using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPIDemo.Extensions;
using RestAPIDemo.Models;
using RestAPIDemo.Models.Extra;
using RestAPIDemo.Services;

namespace RestAPIDemo.Controllers
{
    [ApiController]
    public class UsersController : Controller
    {
        
        private readonly ILoggerManager _logger;    // Global Logger 
        private static IWebHostEnvironment _webHostEnvironment; // Getting Host Enviroment Details 
        private IUserService _userService; // All Logic of CRUD is hidden in UserService implementation

        public UsersController(IUserService userService, ILoggerManager logger, IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }


        // Endpoint to get User List With Sorting, Searching and Paging
        // for sorting, you can pass the following choices: name, country and joiningDate
        // for searching: any valid string
        // for paging: pass the page number, default Page Size is 2
        [HttpGet]
        [Route("api/[controller]")]
        public IActionResult GetUsers(string sortOrder, string searchString, int? pageNumber)
        {

            return Ok(_userService.GetUsers(sortOrder,  searchString, pageNumber ));
        }

        // Endpoint to Get Full details
        [HttpGet]
        [Route("api/[controller]/{guid}")]
        public IActionResult GetUser(Guid guid)
        {
            User user = _userService.GetUser(guid);

            // check if user exist
            if(user != null)
            {
                return Ok(user);
            }
            return NotFound($"The user you queried with id: {guid} does not exist");
        }

        // Endpoint to Add User with Address Information
        [HttpPost]
        [Route("api/[controller]")]
        public IActionResult AddUser(UserDto user)
        {
            if (UserValidatorExtenstion.IsValid(user))
            {
                User createdUser = _userService.AddUser(user);
                return Ok(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + createdUser.UserId);
            }
            else
            {
                return BadRequest("the data is not valid bro");
            }
        }


        // Endpoint to Upload user Profiles, accepting FileUpload DTO and User Guid
        [HttpPost]
        [Route("api/uploadUserProfile")]
        public string UploadUserProfile([FromForm] FileUpload fileUpload, Guid id)
        {
            string ImagePath = "NOT_DEFINED"; // initial value for Image Path
            try
            {
                // check if there is a file in the request
                if (fileUpload.files.Length > 0)
                {
                    // initial the full path of the uploaded file, files will be in wwwroot/uploads/
                    string path = _webHostEnvironment.WebRootPath + "/" + "uploads/";

                    // check if the directry exist, if not create it with the specified path
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    // Start storing file by copying it to the path and update ImagePath with the full path
                    // to be store
                    using (FileStream fileStream = System.IO.File.Create(path + fileUpload.files.FileName))
                    {
                        fileUpload.files.CopyTo(fileStream);
                        fileStream.Flush();
                        ImagePath = path + fileUpload.files.FileName;
                    }


                    // Update User Details with the ImagePath 
                    _userService.UpdateUserProfile(id, ImagePath);
                }
                else
                {
                    // if there was no file, just return NOT_DEFINED value
                    ImagePath = "There is No File in the Request, kindly use 1 File";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ImagePath = "Error Occured During Uploading the File";
            }

            // return the Message to the User
            return ImagePath;
        }


        // Endpoint to delete user, the function accept Guid of the user.
        [HttpDelete]
        [Route("api/[controller]/{guid}")]
        public IActionResult DeleteUser(Guid guid)
        {
            User user = _userService.GetUser(guid);
            if (user != null)
            {
                _userService.DeletUser(user);
                return Ok($"the user is deleted with guid: {guid}");
            }

            return NotFound($"The user you queried with id: {guid} does not exist");

        }

        [HttpPatch]
        [Route("api/[controller]/{guid}")]
        public IActionResult EditUser(Guid guid, UserDto user)
        {
            User ExistingUser = _userService.GetUser(guid);
            if (ExistingUser != null)
            {
                user.UserId = guid;
                _userService.UpdateUser(user);
                return Ok(user);
            }

            return NotFound($"The user you queried with id: {guid} does not exist");

        }
    }
}
