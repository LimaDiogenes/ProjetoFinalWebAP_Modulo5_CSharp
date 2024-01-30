using FinalProj.Entities;

namespace MockDB
{
    public class Users
    {
        public static List<IEntity> UsersList = new List<IEntity>();
        public static string JsonPath = $"{Directory.GetCurrentDirectory()}/users.json";

        static Users()
        {
            JsonIO.ReadJson(JsonPath);
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

        public static List<IEntity> GetUsers()
        {
            return UsersList;
        }

    }
}
