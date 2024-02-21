using Entities;
using Requests;
using System.Collections.Generic;

namespace Entities
{
    public class Cart : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ItemResponse> CartItems { get; set; }

        public Cart(int id)
        {
            Id = id;
            Name = $"cart{id}";
            CartItems = new List<ItemResponse>();
        }
    }
}
