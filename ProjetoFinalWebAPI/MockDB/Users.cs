using FinalProj.Entities;

namespace MockDB
{
    public class Users
    {
        public static List<User> UsersList = new List<User>();
        public static string JsonPath = $"{Directory.GetCurrentDirectory()}\\MockDB\\Assets\\users.json";

        static Users()
        {
            UsersList = JsonIO.ReadJson<User>(JsonPath);
        }

        public static void InsertUser(User user)
        {
            UsersList.Add(user);
            JsonIO.WriteJson(JsonPath, UsersList);
        }

        public static bool DeleteUsers(User user)
        {
            bool valid = UsersList.Remove(user);
            if (valid)
            {
                JsonIO.WriteJson(JsonPath, UsersList);
                return valid;
            }
            return false;
        }

        public static List<User> GetUsers()
        {
            return UsersList;
        }

    }
}
