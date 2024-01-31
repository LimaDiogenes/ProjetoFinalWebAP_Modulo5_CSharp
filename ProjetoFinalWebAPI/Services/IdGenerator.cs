using MockDB;

namespace FinalProj.Services
{
    public static class IdGenerator
    {
        public static int UserId()
        {
            try
            { return Users.UsersList.Max(entity => entity.Id) + 1; }
            catch { return 1; }
        }

        public static int ItemId()
        {
            try
            { return Items.ItemsList.Max(entity => entity.Id) + 1; }
            catch { return 1; }
        }

    }
}
