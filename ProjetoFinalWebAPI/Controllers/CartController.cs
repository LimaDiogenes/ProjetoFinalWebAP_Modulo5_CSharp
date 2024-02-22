using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Mappers;
using Requests;
using System.Security.Claims;


namespace Controllers
{
    [Route("GridironStore/Cart")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        public readonly ICartService cartService;
        public readonly IUserService userService;
        public readonly UserResponse user;

        public CartController(IUserService uService)
        {
            userService = uService;
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            user = userService.GetUserById(int.Parse(userID!))!;
            cartService = new CartService(user);
        }
        [HttpGet]
        public IActionResult Get() 
        {
            return Ok(cartService.GetCartList());
        }
    }
}
