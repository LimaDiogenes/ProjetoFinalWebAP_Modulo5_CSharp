
using System;
using Entities;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Requests;
using Mappers;

namespace MockDB
{

    public interface IItemRepo
    {
        List<Item> ListItems();
        Item? GetById(int id);
        Item? FindByName(string name);
        Item? FindByCategory(string cat);
        Item? FindByAnyField(string search);
        Item CreateItem(Item item);
        Item UpdateItem(Item item);
        bool DeleteItem(ItemResponse item);
    }

    internal class ItemsRepo : IItemRepo
    {
        public static List<Item> ItemsList = new List<Item>();
        public static string JsonPath = $"{Directory.GetCurrentDirectory()}\\MockDB\\Assets\\items.json";

        static ItemsRepo()
        {
            ItemsList = JsonIO.ReadJson<Item>(JsonPath);
        }

        public Item CreateItem(Item item)
        {
            ItemsList.Add(item);
            JsonIO.WriteJson(JsonPath, ItemsList);
            return item;
        }

        public bool DeleteItem(ItemResponse itemResponse)
        {
            var item = GetById(itemResponse.Id);
            
            bool valid = ItemsList.Remove(item!);
            if (valid)
            {
                JsonIO.WriteJson(JsonPath, ItemsList);
                return valid;
            }
            return false;
        }

        public List<Item> ListItems()
        {
            return ItemsList;
        }

        public Item? FindByAnyField(string search)
        {
            throw new NotImplementedException();
        }

        public Item? FindByCategory(string cat)
        {
            throw new NotImplementedException();
        }

        public Item? FindByName(string name)
        {
            throw new NotImplementedException();
        }

        public Item? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Item UpdateItem(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
