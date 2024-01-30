using FinalProj.Entities;
using System.Text.Json;

namespace MockDB
{
    public static class JsonIO
    {
        public static List<IEntity> ReadJson(string jsonPath)
        {
            if (File.Exists(jsonPath))
            {
                using StreamReader reader = File.OpenText(jsonPath);
                string jsonString = reader.ReadToEnd();
                return JsonSerializer.Deserialize<List<IEntity>>(jsonString);
            }
            return [];
        }

        public static void WriteJson(string jsonPath, List<IEntity> list)
        {
            using (StreamWriter writer = File.CreateText(jsonPath))
            {
                writer.Write(JsonSerializer.Serialize(list));
            };
        }
    }
}
