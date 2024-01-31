using FinalProj.Entities;
using Newtonsoft.Json;

namespace MockDB
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


        public static void WriteJson(string jsonPath, List<Item> list)
        {
            using (StreamWriter writer = File.CreateText(jsonPath))
            {
                writer.Write(System.Text.Json.JsonSerializer.Serialize(list));
            };
        }

        public static void WriteJson(string jsonPath, List<User> list)
        {
            using (StreamWriter writer = File.CreateText(jsonPath))
            {
                writer.Write(System.Text.Json.JsonSerializer.Serialize(list));
            };
        }
    }
}
