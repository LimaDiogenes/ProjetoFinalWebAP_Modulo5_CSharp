
namespace FinalProj.Entities
{
    public class Item : IEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Category { get; private set; }
        public string Variant { get; private set; }
        public string Size { get; private set; }
        public double Price { get; private set; }

        public Item(string name, string category, double price, string variant, string size)
        {
            Id = IdGenerator.ItemId();
            Name = name;
            Category = category; 
            Price = price;
            Variant = variant;
            Size = size;
        }
    }
}
