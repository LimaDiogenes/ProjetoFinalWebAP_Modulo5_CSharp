using MockDB;
using System.Linq;

namespace Services
{
    public static class IdGenerator
    {
        public static int UserId()
        {
            try
            { return UsersRepo.UsersList.Max(entity => entity.Id) + 1; }
            catch { return 1; }
        }

        public static int ItemId()
        {
            try
            { return ItemsRepo.ItemsList.Max(entity => entity.Id) + 1; }
            catch { return 1; }
        }

    }
}
