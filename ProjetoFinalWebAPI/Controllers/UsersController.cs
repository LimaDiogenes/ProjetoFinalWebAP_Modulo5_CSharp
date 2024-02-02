using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockDB;
using FinalProj;
using Services;
using Microsoft.AspNetCore.Authorization;
using Requests;
using Validators;

namespace Controllers
{
    [Route("GridironStore/Login")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly IAuthService _authService;
        public UserLoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public IActionResult SignIn([FromForm] AuthRequest request)
        {
            var response = _authService.SignIn(request);
            return Ok(response);
        }
    }

    [Route("GridironStore/Users")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService service)
        {
            userService = service;
        }

               
        [HttpGet("ListAllUsers")]        
        public IActionResult List()
        {
            return Ok(userService.ListUsers());
        }

        
        [HttpGet("Profile/{id}")]
        public IActionResult Get(int id)
        {
            var user = userService.GetUserById(id);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpPost("Profile/Register")]
        [AllowAnonymous]
        public IActionResult Post([FromBody] BaseUserRequest user)
        {
            var newUser = userService.CreateUser(user);
            return Ok(newUser);
        }

        [HttpPut("Profile/Update/{id}")]
        public IActionResult Put(int id, [FromBody] ToUserResponse user)
        {
            user.Id = id;
            var updatedUser = userService.UpdateUser(user);
            return Ok(updatedUser);
        }

        [HttpDelete("Profile/Delete/{id}")]
        public IActionResult Delete(int id)
        {            
            var user = userService.GetUserById(id);
            var control = userService.DeleteUser(id);
            return control ? Ok($"User {user!.Name} deleted successfully.") : NotFound("User not found or deletion failed.");
        }
    }
}
