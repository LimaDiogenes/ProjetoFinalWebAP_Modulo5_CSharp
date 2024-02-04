using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockDB;
using FinalProj;
using Services;
using Microsoft.AspNetCore.Authorization;
using Requests;
using Validators;
using System;
using System.Security.Claims;
using Exceptions;

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
    
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;

        public UsersController(IUserService service)
        {
            userService = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("ListAllUsers")]        
        public IActionResult List()
        {
            return Ok(userService.ListUsers());
        }

        
        [HttpGet("Profile/{id}")]
        [Authorize]
        public IActionResult Get(int id)
        {
            // preciso alterar esta parte abaixo para tirar a logica daqui:
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var isAdmin = User.FindFirst(ClaimTypes.Role)!.Value.ToString();
            if (int.Parse(userId) != id && isAdmin != "Admin")
            { throw new UnathorizedException("Only Admin users can access other users"); }

            var user = userService.GetUserById(id);
            return Ok(user);
        }

        [HttpPost("Profile/Register")]
        [AllowAnonymous]
        public IActionResult Post([FromBody] BaseUserRequest user)
        {
            var newUser = userService.CreateUser(user);
            return Ok(newUser);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Profile/Update/{id}")]
        public IActionResult Put(int id, [FromBody] ToUserResponse user)
        {
            user.Id = id;
            var updatedUser = userService.UpdateUser(user);
            return Ok(updatedUser);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Profile/Delete/{id}")]
        public IActionResult Delete(int id)
        {            
            var user = userService.GetUserById(id);
            var control = userService.DeleteUser(id);
            return control ? Ok($"User {user!.Name} deleted successfully.") : NotFound("User not found or deletion failed.");
        }
    }
}
