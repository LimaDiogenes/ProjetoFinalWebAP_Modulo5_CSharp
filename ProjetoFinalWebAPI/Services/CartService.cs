using MockDB;
using Requests;
using System.Collections.Generic;
using Entities;

namespace Services
{
    public interface ICartService
    {
        /// <summary>
        /// Retrieves the list of items in the user's cart.
        /// </summary>
        /// <returns>The list of items in the cart.</returns>
        List<ItemResponse>? GetCartList();

        /// <summary>
        /// Adds an item to the user's cart.
        /// </summary>
        /// <param name="request">The request containing details of the item to add.</param>
        /// <returns>The updated list of items in the cart.</returns>
        List<ItemResponse>? AddToCart(BaseItemRequest request);

        /// <summary>
        /// Removes an item from the user's cart.
        /// </summary>
        /// <param name="request">The request containing details of the item to remove.</param>
        /// <returns>The updated list of items in the cart.</returns>
        List<ItemResponse>? RemoveFromCart(BaseItemRequest request);

        /// <summary>
        /// Updates the quantity of an item in the user's cart.
        /// </summary>
        /// <param name="request">The request containing details of the item to update.</param>
        /// <param name="newQuantity">The new quantity for the item.</param>
        /// <returns>The updated list of items in the cart.</returns>
        List<ItemResponse>? UpdateItemQuantity(BaseItemRequest request, int newQuantity);
    }

    public class CartService : ICartService
    {
        public CartRepo UserCart { get; set; }
        public CartService(UserResponse user)
        {
            var cart = new Cart(user.Id);
            UserCart = new CartRepo(cart);
        }

        public List<ItemResponse>? GetCartList()
        {
            return UserCart.ListCartItems();
        }

        public List<ItemResponse>? AddToCart(BaseItemRequest request)
        {
            return UserCart.AddItemToCart(request);
        }

        public List<ItemResponse>? RemoveFromCart(BaseItemRequest request)
        {
            return UserCart.RemoveItem(request);
        }

        public List<ItemResponse>? UpdateItemQuantity(BaseItemRequest request, int newQuantity)
        {
            return UserCart.UpdateItemQuantity(request.Id, newQuantity);
        }
    }
}
