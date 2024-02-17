
using System;
using Entities;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Requests;
using Mappers;
using System.Linq;
using Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace MockDB
{

    public interface IItemRepo
    {
        List<ItemResponse>? ListItems();
        ItemResponse? GetById(int id);
        ItemResponse CreateItem(BaseItemRequest item);
        ItemResponse? UpdateItem(BaseItemRequest item, int id);
        bool DeleteItem(BaseItemRequest item);
        bool DeleteItem(ItemResponse item);
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
                ItemsList.Add(item);
            }
        }

        public ItemResponse CreateItem(BaseItemRequest item)
        {
            var newItem = ItemMapper.ToEntity(item);

            var checkDuplicates = ItemsList.Any(i => i.Name == item.Name &&
                                                        i.Variant == item.Variant &&
                                                        i.Size == item.Size);
            if (checkDuplicates)  throw new DuplicateException("Item already in database!");
            
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

        public bool DeleteItem(ItemResponse item)
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
            return ItemsList?.Select(item => ItemMapper.ToResponse(item)).ToList();
        }

        public ItemResponse? UpdateItem(BaseItemRequest item, int id)
        {            
            var itemToUpdate = ItemsList.FirstOrDefault(i => i.Id == id);
            if (itemToUpdate != null)
            {
                if (item.Name.IsNullOrEmpty() && item.Variant.IsNullOrEmpty() &&
                    (item.Price == 0 || item.Price.ToString().IsNullOrEmpty()) && item.Size.IsNullOrEmpty() && item.Category.IsNullOrEmpty())
                {
                    throw new BadRequestException("At least one field is required to update the item! " +
                                                  "Fields: Name, Variant, Price, Size, Category");
                };
                
                if (item.Name.IsNullOrEmpty()) item.Name = itemToUpdate.Name;
                if (item.Variant.IsNullOrEmpty()) item.Variant = itemToUpdate.Variant;
                if (item.Price == 0 || item.Price.ToString().IsNullOrEmpty()) item.Price = itemToUpdate.Price;
                if (item.Size.IsNullOrEmpty()) item.Size = itemToUpdate.Size;
                if (item.Category.IsNullOrEmpty()) item.Category = itemToUpdate.Category;
                                                
                var newItem = ItemMapper.ToEntity(item);
                newItem.Id = id;

                ItemsList.Remove(itemToUpdate);
                ItemsList.Add(newItem);
                JsonIO.WriteJson(JsonPath, ItemsList);
                return ItemMapper.ToResponse(newItem);
            }
            return null;
        }
    }
}
