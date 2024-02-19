using Entities;
using Requests;
using System.Text;



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
            Quantity = item.Quantity,
            EanCode = item.EanCode,
        };

        public static Item ToEntity(BaseItemRequest item) => new Item(
            name: item.Name!,
            category: item.Category!,
            variant: item.Variant!,
            size: item.Size!,
            price: item.Price,
            quantity: item.Quantity,
            eanCode: item.EanCode!
        );

        public static Item ToEntity(ItemResponse item) => new Item(
            id: item.Id!,
            name: item.Name!,
            category: item.Category!,
            variant: item.Variant!,
            size: item.Size!,
            price: item.Price,
            quantity: item.Quantity,
            eanCode: item.EanCode
        );

        public static Item ToEntity(ToItemResponse item) => new Item(
            name: item.Name!,
            category: item.Category!,
            variant: item.Variant!,
            size: item.Size!,
            price: item.Price,
            quantity: item.Quantity,
            eanCode: item.EanCode!
        );
    }
}
