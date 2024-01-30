
using FinalProj.Entities;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using System.Text.Json;

namespace MockDB
{
    internal static class Items
    {
        public static List<IEntity> ItemsList = new List<IEntity>();
        public static string JsonPath = $"{Directory.GetCurrentDirectory()}/items.json";

        static Items()
        {            
            JsonIO.ReadJson(JsonPath);
        }

        public static void InsertItem(Item item)
        {
            ItemsList.Add(item);
            JsonIO.WriteJson(JsonPath, ItemsList);
        }
        public static bool DeleteItem(Item item)
        {
            bool valid = ItemsList.Remove(item);
            if (valid)
            {
                JsonIO.WriteJson(JsonPath, ItemsList);
                return valid;
            }
            return false;
        }

        public static List<IEntity> GetItems()
        {
            return ItemsList;
        }

    }
}
