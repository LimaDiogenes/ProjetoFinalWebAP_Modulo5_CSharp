using Entities;
using Requests;



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
            name: item.Name!,
            category: item.Category!,
            variant: item.Variant!,
            size: item.Size!,
            price: item.Price
        );

        public static Item ToEntity(ToItemResponse item) => new Item(
            name: item.Name!,
            category: item.Category!,
            variant: item.Variant!,
            size: item.Size!,
            price: item.Price
        );
    }
}
