﻿
using FinalProj.Entities;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;
using System.Text.Json;

namespace MockDB
{
    internal static class Items
    {
        public static List<Item> ItemsList = new List<Item>();
        public static string JsonPath = $"{Directory.GetCurrentDirectory()}\\MockDB\\Assets\\items.json";

        static Items()
        {
            ;
        }

        public static void Init()
        {
            ItemsList = JsonIO.ReadJson<Item>(JsonPath);
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

        public static List<Item> GetItems()
        {
            return ItemsList;
        }

    }
}
