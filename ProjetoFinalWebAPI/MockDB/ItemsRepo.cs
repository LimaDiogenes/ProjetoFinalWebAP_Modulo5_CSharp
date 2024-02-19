using Entities;
using Exceptions;
using Mappers;
using Microsoft.IdentityModel.Tokens;
using Requests;
using System.Collections.Generic;
using System.IO;
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
        bool DeleteItem(ItemResponse item);
        bool HasDuplicateId(Item newItem);
        bool HasDuplicateEAN(Item newItem);        
    }

    internal class ItemsRepo : IItemRepo
    {
        public static List<Item> ItemsList = new List<Item>();
        public static string JsonPath = $"{Directory.GetCurrentDirectory()}\\MockDB\\Assets\\items.json";

        static ItemsRepo()
        {
            var readList = ReadFromDB();
            if (!readList.IsNullOrEmpty())
            {
                foreach (var item in readList!)
                {
                    ItemsList.Add(item);
                }
            }
        }

        public ItemResponse CreateItem(BaseItemRequest item)
        {
            ItemsList = ReadFromDB();
            var newItem = ItemMapper.ToEntity(item);
            HasDuplicateEAN(newItem);
            HasDuplicateId(newItem);

            var checkDuplicates = ItemsList.Any(i => i.Name == item.Name &&
                                                        i.Variant == item.Variant &&
                                                        i.Size == item.Size);
            if (checkDuplicates) throw new DuplicateException("Item already in database!");

            ItemsList!.Add(newItem);
            JsonIO.WriteJson(JsonPath, ItemsList);
            return ItemMapper.ToResponse(newItem);
        }

        public bool DeleteItem(BaseItemRequest item)
        {
            ItemsList = ReadFromDB();
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
            ItemsList = ReadFromDB();
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
            ItemsList = ReadFromDB();
            var response = ItemsList.FirstOrDefault(i => i.Id == id);
            return response != null ? ItemMapper.ToResponse(response) : null;
        }

        public List<ItemResponse>? ListItems()
        {
            ItemsList = ReadFromDB();
            return ItemsList?.Select(item => ItemMapper.ToResponse(item)).ToList();
        }

        public ItemResponse? UpdateItem(BaseItemRequest item, int id)
        {
            ItemsList = ReadFromDB();
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
                HasDuplicateEAN(newItem);
                newItem.Id = id;

                ItemsList.Remove(itemToUpdate);
                ItemsList.Add(newItem);
                JsonIO.WriteJson(JsonPath, ItemsList);
                return ItemMapper.ToResponse(newItem);
            }
            return null;
        }

        /// <summary>
        /// Returns true if any duplicate is found, false otherwise.
        /// Do not use for updating items.
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        public bool HasDuplicateId(Item newItem)
        {
            ItemsList = ReadFromDB();
            return ItemsList.Any(item => item.Id == newItem.Id);
        }

        /// <summary>
        /// Returns true if any duplicate is found, false otherwise
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        public bool HasDuplicateEAN(Item newItem) 
        {
            ItemsList = ReadFromDB();
            return ItemsList.Any(item => item.EanCode == newItem.EanCode);
        }

        private static List<Item>? ReadFromDB() 
        {
            return JsonIO.ReadJson<Item>(JsonPath);
        }
    }
}
