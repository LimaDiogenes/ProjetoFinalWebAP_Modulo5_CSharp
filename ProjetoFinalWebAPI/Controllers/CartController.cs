using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Requests;
using Services;
using System.Security.Claims;
using Mappers;


namespace Controllers
{
    [Route("GridironStore/Cart")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        // nao consegui resgatar info do usuario do token no construtor ( metodo GetUserFromJwt() ), por isso está sendo inicializado um novo CartService em cada metodo

        public readonly IUserService userService;
        private readonly IItemService itemService;
        public CartController(IUserService userService, IItemService itemService)
        {
            this.userService = userService;
            this.itemService = itemService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var user = GetUserFromJwt();
            var cartList = new CartService(user).GetCart();

            return Ok(!cartList.IsNullOrEmpty() ? cartList : new { message = "Cart is empty", list = cartList });
        }

        [HttpPut]
        public IActionResult Put([FromForm] int itemId, [FromForm] int newQuantity)
        {
            var user = GetUserFromJwt();
            var cartService = new CartService(user);
            var updatedList = cartService.UpdateItemQuantity(itemId, newQuantity);

            return Ok( new { updated_list = updatedList } );
        }
        [HttpDelete]
        public IActionResult Delete(int itemId)
        {
            var user = GetUserFromJwt();
            var cartService = new CartService(user);
            var item = itemService.GetItemById(itemId);
            var updatedList = cartService.RemoveFromCart(item!);
            return Ok(!updatedList.IsNullOrEmpty() ? updatedList : new { message = "Cart is empty", list = updatedList});
        }
        private UserResponse GetUserFromJwt()
        {
            int userID;
            try
            {
                userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            }
            catch (System.Exception)
            {
                throw new Exceptions.NotFoundException("Invalid token information - User not found");
            }

            return userService.GetUserById(userID)!;
        }
    }
}
