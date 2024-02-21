using Entities;
using MockDB;
using Requests;

namespace Mappers
{
    public class CartMapper
    {
        public static CartResponse ToResponse(CartRepo repo) => new CartResponse
        { 
            Id = repo.CartId,
            ItemsList = repo.ListCartItems(),
        };
       
    }
}
