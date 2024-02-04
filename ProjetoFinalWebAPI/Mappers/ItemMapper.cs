using Entities;
using Requests;
using System.Drawing;
using System.Xml.Linq;


namespace Mappers
{
    public class ItemMapper
    {
        public static ItemResponse ToResponse(Item item) => new ItemResponse
        {
            Id = item.Id,
            Name = item.Name,
            Category = item.Category,
            Variant = item.Variant,
            Size = item.Size,
            Price = item.Price,
        };

        public static Item ToEntity(BaseItemRequest item) => new Item(
            Name = item.Name,
            Category = item.Category,
            Variant = item.Variant,
            Size = item.Size,
            Price = item.Price,
        );

        public static User ToEntity(ToUserResponse user) => new User(
            name: user.Name!,
            email: user.Email!,
            admin: user.Admin,
            password: user.Password!
        );
    }
}
