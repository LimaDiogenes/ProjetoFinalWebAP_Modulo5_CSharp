
using ProjetoFinalWebAPI;

namespace MockDB
{
    internal class Items
    {
        public static List<Item> ItemsList = new List<Item>(); 
        public static bool AddItem(Item item)
        {
            ItemsList.Add(item);
        }
        public static bool RemoveItem(Item item)
        {
            ItemsList.Remove(item);
        }
    }
}
