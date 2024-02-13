using Exceptions;
using Requests;
using Validators;
using MockDB;
using Mappers;
using System.Collections.Generic;
using System.Linq;

namespace Services
{

    public interface IItemService
    {
        List<ItemResponse> ListItems();
        ItemResponse? GetItemById(int id);
        List<ItemResponse>? GetItemsByName(string name);
        List<ItemResponse>? GetItemsByCategory(string category);
        List<ItemResponse>? GetItemsByAnyField(string search);
        ItemResponse CreateItem(BaseItemRequest newItem);
        ItemResponse UpdateItem(BaseItemRequest updatedItem);
        bool DeleteItem(int id);
    }
    public class ItemService : IItemService
    {
        private readonly IValidator<BaseItemRequest> _validator;
        private readonly IItemRepo _repo;

        public ItemService(IItemRepo repo, IValidator<BaseItemRequest> validator)
        {
            _repo = repo;
            _validator = validator;
        }

        public ItemResponse CreateItem(BaseItemRequest newItem)
        {
            var errors = _validator.Validate(newItem);

            if (errors.Any())
                throw new BadRequestException(errors);

            var item = ItemMapper.ToEntity(newItem);
            var mappedItem = _repo.CreateItem(item);
            return ItemMapper.ToResponse(mappedItem);
        }

        public bool DeleteItem(int id)
        {
            var item = GetItemById(id);
            return _repo.DeleteItem(item!);
        }

        public ItemResponse? GetItemById(int id)
        {
            var item = _repo.GetById(id);
            return item is null ? null : ItemMapper.ToResponse(item);
        }

        public List<ItemResponse>? GetItemsByAnyField(string search)
        {
            List<ItemResponse>? itemsList = null;
            foreach (var item in _repo.ListItems())
            {
                if (item.Id.ToString().Contains(search) ||                     
                    item.Name.Contains(search) ||
                    item.Category.Contains(search) ||
                    item.Variant.Contains(search) ||
                    item.Size.Contains(search))
                {
                    itemsList!.Add(ItemMapper.ToResponse(item));
                }
            }

            return itemsList;
        }

        public List<ItemResponse>? GetItemsByCategory(string category)
        {
            List<ItemResponse>? itemsList = null;
            foreach (var item in _repo.ListItems())
            {
                if (item.Category.Contains(category))                   
                {
                    itemsList!.Add(ItemMapper.ToResponse(item));
                }
            }

            return itemsList;
        }

        public List<ItemResponse>? GetItemsByName(string name)
        {
            List<ItemResponse>? itemsList = null;
            foreach (var item in _repo.ListItems())
            {
                if (item.Name.Contains(name))
                {
                    itemsList!.Add(ItemMapper.ToResponse(item));
                }
            }

            return itemsList;
        }

        public List<ItemResponse> ListItems()
        {
            throw new System.NotImplementedException();
        }

        public ItemResponse UpdateItem(BaseItemRequest updatedItem)
        {
            throw new System.NotImplementedException();
        }
    }
}
