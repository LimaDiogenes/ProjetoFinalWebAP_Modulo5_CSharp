
using System;
using Entities;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Requests;
using Mappers;
using System.Linq;

namespace MockDB
{

    public interface IItemRepo
    {
        List<ItemResponse>? ListItems();
        ItemResponse? GetById(int id);
        ItemResponse CreateItem(BaseItemRequest item);
        ItemResponse? UpdateItem(BaseItemRequest item, int id);
        bool DeleteItem(BaseItemRequest item);
    }

    internal class ItemsRepo : IItemRepo
    {
        public static List<Item> ItemsList = new List<Item>();
        public static string JsonPath = $"{Directory.GetCurrentDirectory()}\\MockDB\\Assets\\items.json";

        static ItemsRepo()
        {
            var readList = JsonIO.ReadJson<Item>(JsonPath);
            foreach (var item in readList)
            {
                ItemMapper.ToResponse(item);
            }
        }

        public ItemResponse CreateItem(BaseItemRequest item)
        {
            var newItem = ItemMapper.ToEntity(item);
            ItemsList!.Add(newItem);
            JsonIO.WriteJson(JsonPath, ItemsList);
            return ItemMapper.ToResponse(newItem);
        }

        public bool DeleteItem(BaseItemRequest item)
        {
            var responseToDelete = GetById(item.Id);
            var itemToDelete = ItemsList.FirstOrDefault(i => i.Id == item.Id);

            bool valid = ItemsList.Remove(itemToDelete!);
            if (valid)
            {
                JsonIO.WriteJson(JsonPath, ItemsList);
                return valid;
            }
            return false;
        }

        public ItemResponse? GetById(int id)
        {
            var response = ItemsList.FirstOrDefault(i => i.Id == id);
            return response != null ? ItemMapper.ToResponse(response) : null;
        }

        public List<ItemResponse>? ListItems()
        {
            List<ItemResponse>? respList = null;
            foreach (var item in ItemsList)
            {
                respList!.Add(ItemMapper.ToResponse(item));
            }

            return respList != null ? respList : null;
        }

        public ItemResponse? UpdateItem(BaseItemRequest item, int id)
        {            
            var itemToUpdate = ItemsList.FirstOrDefault(i => i.Id == id);
            if (itemToUpdate != null)
            {
                ItemsList.Remove(itemToUpdate);
                var newItem = ItemMapper.ToEntity(item);
                newItem.Id = id;
                ItemsList.Add(newItem);
                JsonIO.WriteJson(JsonPath, ItemsList);
                return ItemMapper.ToResponse(newItem);
            }
            return null;
        }
    }
}
