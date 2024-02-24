using MockDB;
using Requests;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;
using Mappers;


namespace Services
{
    public interface ICartService
    {
        /// <summary>
        /// Retrieves the list of items in the user's cart.
        /// </summary>
        /// <returns>The list of items in the cart.</returns>
        List<ItemResponse>? GetCart();

        /// <summary>
        /// Adds an item to the user's cart.
        /// </summary>
        /// <param name="request">The request containing details of the item to add.</param>
        /// <returns>The updated list of items in the cart.</returns>
        List<ItemResponse>? AddToCart(ItemResponse request);

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
            UserCart = new CartRepo(user.Id);            
        }

        public List<ItemResponse>? GetCart()
        {
            return UserCart.GetCart();
        }

        public List<ItemResponse>? AddToCart(ItemResponse item)
        {
            return UserCart.AddItemToCart(item);
        }

        public List<ItemResponse>? RemoveFromCart(BaseItemRequest request)
        {
            return UserCart.RemoveItem(request);
        }

        public List<ItemResponse>? RemoveFromCart(ItemResponse response)
        {
            return UserCart.RemoveItem(response);
        }

        public List<ItemResponse>? UpdateItemQuantity(BaseItemRequest request, int newQuantity)
        {
            return UserCart.UpdateItemQuantity(request.Id, newQuantity);
        }

        public List<ItemResponse>? UpdateItemQuantity(int itemId, int newQuantity)
        {
            return UserCart.UpdateItemQuantity(itemId, newQuantity);
        }
    }
}
