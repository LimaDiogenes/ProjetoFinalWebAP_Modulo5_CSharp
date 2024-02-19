
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
        public int Quantity { get; set; }
        public string EanCode { get; set; }


        public Item(string name, string category, double price, string variant, string size, string eanCode, int quantity = 0, int id = 0)
        {
            Id = (id == 0) ? IdGenerator.ItemId() : id;
            Quantity = quantity;
            Name = name;
            Category = category; 
            Price = price;
            Variant = variant;
            Size = size;
            EanCode = eanCode;
        }
    }
}
