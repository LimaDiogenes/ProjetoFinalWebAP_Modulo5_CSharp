using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Services
{
    public static class JsonIO
    {
        public static List<T> ReadJson<T>(string jsonPath) where T : IEntity
        {
            if (File.Exists(jsonPath))
            {
                string jsonString = File.ReadAllText(jsonPath);
                return JsonConvert.DeserializeObject<List<T>>(jsonString)!;
            }
            return [];
        }

        /// <summary>
        /// Returns true if writing to JSON was successful, false otherwise
        /// </summary>
        /// <param name="jsonPath"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool WriteJson(string jsonPath, List<Item> list)
        {
            try
            {
                using (StreamWriter writer = File.CreateText(jsonPath))
                {
                    writer.Write(System.Text.Json.JsonSerializer.Serialize(list));
                    return true;
                };
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if writing to JSON was successful, false otherwise
        /// </summary>
        /// <param name="jsonPath"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool WriteJson(string jsonPath, List<User> list)
        {
            try
            {
                using (StreamWriter writer = File.CreateText(jsonPath))
                {
                    writer.Write(System.Text.Json.JsonSerializer.Serialize(list));
                    return true;
                };
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
