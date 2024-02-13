
using Services;

namespace Entities
{
    public class Item : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Variant { get; set; }
        public string Size { get; set; }
        public double Price { get; set; }

        public Item(string name, string category, double price, string variant, string size, int id = 0)
        {
            Id = (id == 0) ? IdGenerator.ItemId() : id;
            Name = name;
            Category = category; 
            Price = price;
            Variant = variant;
            Size = size;
        }
    }
}
