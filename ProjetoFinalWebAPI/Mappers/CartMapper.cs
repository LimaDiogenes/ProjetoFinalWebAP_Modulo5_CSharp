using Entities;
using MockDB;
using Requests;

namespace Mappers
{
    public class CartMapper
    {
        public static CartResponse ToResponse(Cart cart) => new CartResponse
        { 
            Id = cart.Id,
            ItemsList = cart.CartItems,
        };
       
    }
}
