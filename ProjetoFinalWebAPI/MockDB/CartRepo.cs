using Entities;
using Mappers;
using Microsoft.IdentityModel.Tokens;
using Requests;
using Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Exceptions;
using System;


namespace MockDB
{
    public interface ICartRepo
    {
        /// <summary>
        /// Lists all items in the cart.
        /// </summary>
        /// <returns>The list of items in the cart.</returns>
        List<ItemResponse>? ListCartItems();
        /// <summary>
        /// Adds an item to the cart and returns the updated list of items.
        /// </summary>
        /// <param name="item">The item to add to the cart.</param>
        /// <returns>The updated list of items in the cart.</returns>
        List<ItemResponse>? AddItemToCart(BaseItemRequest item);
        /// <summary>
        /// Updates the quantity of an item in the cart and returns the updated list of items.
        /// </summary>
        /// <param name="itemId">The ID of the item to update.</param>
        /// <param name="newQuantity">The new quantity for the item.</param>
        /// <returns>The updated list of items in the cart.</returns>
        /// <exception cref="BadRequestException">Thrown when the item with the given ID is not found in the cart.</exception>
        List<ItemResponse>? UpdateItemQuantity(int itemId, int newQuantity);
        /// <summary>
        /// Removes an item from the cart and returns the updated list of items.
        /// </summary>
        /// <param name="item">The item to remove from the cart.</param>
        /// <returns>The updated list of items in the cart.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the cart is empty.</exception>
        /// <exception cref="BadRequestException">Thrown when the item is not found in the cart.</exception>
        List<ItemResponse>? RemoveItem(BaseItemRequest item);
    }

    public class CartRepo : ICartRepo
    {
        public Cart _Cart { get; set; }
        public List<ItemResponse> ItemsList { get; set; } = new();
        public string JsonPath { get; set; }

        public CartRepo(Cart cart)
        {
            var readList = ReadFromDB();
            if (!readList.IsNullOrEmpty())
            {
                foreach (var item in readList!)
                {
                    ItemsList.Add(item);
                }
            }
            _Cart = cart;
            JsonPath = $"{Directory.GetCurrentDirectory()}\\MockDB\\Assets\\Carts\\cart{_Cart.Id}.json";
        }

        #region privateMethods
        /// <summary>
        /// Reads the list of items from the JSON file.
        /// </summary>
        /// <returns>The list of items read from the JSON file.</returns>
        private List<ItemResponse>? ReadFromDB()
        {
            var entitiesList = JsonIO.ReadJson<Item>(JsonPath);
            if (entitiesList.IsNullOrEmpty())
                return null;
            ItemsList = entitiesList.Select(ent => ItemMapper.ToResponse(ent)).ToList();
            return ItemsList;
        }

        /// <summary>
        /// Writes the list of items to the JSON file.
        /// </summary>
        /// <returns><c>true</c> if the operation was successful; otherwise, <c>false</c>.</returns>
        private bool WriteToDb()
        {
            var newList = ItemsList.Select(i => ItemMapper.ToEntity(i)).ToList();
            return JsonIO.WriteJson(JsonPath, newList);
        }
        #endregion 
        public List<ItemResponse>? AddItemToCart(BaseItemRequest item)
        {
            ReadFromDB();
            var newItem = ItemMapper.ToEntity(item);
            var newItemResponse = ItemMapper.ToResponse(newItem);
            ItemsList.Add(newItemResponse);
            WriteToDb();
            return ItemsList;
        }

        public List<ItemResponse>? ListCartItems()
        {
            ReadFromDB();
            return ItemsList.IsNullOrEmpty() ? null : ItemsList;
        }

        public List<ItemResponse>? RemoveItem(BaseItemRequest item)
        {
            ReadFromDB();
            if (ItemsList.IsNullOrEmpty())
                throw new InvalidOperationException("Cart is empty");

            ItemResponse? itemToRemove = ItemsList.FirstOrDefault(i => i.Id == item.Id);
            if (itemToRemove == null)            
                throw new BadRequestException("Item not found in cart");
                                  
            ItemsList.Remove(itemToRemove);
            WriteToDb();
            return ItemsList;                
        }

        public List<ItemResponse>? UpdateItemQuantity(int itemId, int newQuantity)
        {
            ReadFromDB();

            try
            {
                ItemsList[ItemsList.FindIndex(i => i.Id == itemId)].Quantity = newQuantity;
            }
            catch (Exception) 
            {
                throw new BadRequestException("Item with given itemId not found in cart");
            }

            WriteToDb();
            return ItemsList;
        }
    }
}
