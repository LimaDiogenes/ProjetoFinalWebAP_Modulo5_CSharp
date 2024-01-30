using MockDB;

namespace FinalProj.Entities
{
    public static class IdGenerator
    {
        public static int UserId()
        {            
            return MockDB.Users.UsersList.Max(entity => entity.Id) + 1;
        }

        public static int ItemId()
        {                  
            return MockDB.Items.ItemsList.Max(entity => entity.Id) + 1;
        }

    }
}
