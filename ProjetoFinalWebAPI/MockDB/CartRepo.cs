using Entities;
using Exceptions;
using Services;
using Mappers;
using Microsoft.IdentityModel.Tokens;
using Requests;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MockDB
{

    public interface ICartRepo
    {
        List<ItemResponse>? ListCartItems();        
        ItemResponse AddItemToCart(BaseItemRequest item);
        ItemResponse? UpdateItemQuantity(BaseItemRequest item, int newQuantity);
        bool RemoveItem(BaseItemRequest item);      
    }

    internal class CartRepo : ICartRepo
    {   
        public required int CartId { get; set; }
        public List<Item> ItemsList = new List<Item>();
        public string JsonPath { get; set; }

        public CartRepo(int userId)
        {
            var readList = ReadFromDB();
            if (!readList.IsNullOrEmpty())
            {
                foreach (var item in readList!)
                {
                    ItemsList.Add(item);
                }
            }
            CartId = userId;
            JsonPath = $"{Directory.GetCurrentDirectory()}\\MockDB\\Assets\\carts{CartId}.json";
        }

        private List<Item>? ReadFromDB()
        {
            return JsonIO.ReadJson<Item>(JsonPath);
        }

        private bool WriteToDb()
        {
            return JsonIO.WriteJson(JsonPath, ItemsList);
        }

        public ItemResponse AddItemToCart(BaseItemRequest item)
        {
            var newItem = ItemMapper.ToEntity(item);
            ItemsList.Add(newItem);
            return ItemMapper.ToResponse(newItem);            
        }

        public List<Item>? ListCartItems()
        {
            return ;
        }

        public bool RemoveItem(BaseItemRequest item)
        {
            throw new System.NotImplementedException();
        }

        public ItemResponse? UpdateItemQuantity(BaseItemRequest item, int newQuantity)
        {
            throw new System.NotImplementedException();
        }
    }
}
