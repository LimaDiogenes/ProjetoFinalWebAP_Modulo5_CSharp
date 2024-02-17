using Exceptions;
using Mappers;
using Microsoft.IdentityModel.Tokens;
using MockDB;
using Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using Validators;

namespace Services
{

    public interface IItemService
    {
        List<ItemResponse>? ListItems();
        ItemResponse? GetItemById(int id);
        List<ItemResponse>? GetItemsByName(string name);
        List<ItemResponse>? GetItemsByCategory(string category);
        List<ItemResponse>? GetItemsByAnyField(string search);
        ItemResponse? CreateItem(BaseItemRequest newItem);
        ItemResponse? UpdateItem(BaseItemRequest updatedItem);
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

            return _repo.CreateItem(newItem);
        }

        public bool DeleteItem(int id)
        {
            var item = GetItemById(id)!;      
            return _repo.DeleteItem(item);
        }

        public ItemResponse? GetItemById(int id)
        {
            var item = _repo.GetById(id);
            return item is null ? null : item;
        }

        public List<ItemResponse>? GetItemsByAnyField(string search)
        {
            List<ItemResponse>? itemsList = null;
            var repoList = _repo.ListItems();
            if (repoList != null)
            {
                itemsList = repoList.FindAll(i => i.Id.ToString().Equals(search, StringComparison.OrdinalIgnoreCase) ||
                                                            i.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                                            i.Variant.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                                            i.Category.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                                            i.Size.Contains(search, StringComparison.OrdinalIgnoreCase));
            };
    
            return itemsList;
        }

        public List<ItemResponse>? GetItemsByCategory(string category)
        {
            List<ItemResponse>? itemsList = null;
            var repoList = _repo.ListItems();

            if (!repoList.IsNullOrEmpty())
            {
                itemsList = _repo.ListItems()!.Where(item => item.Category.Contains(category)).ToList();
            }

            return itemsList;
        }

        public List<ItemResponse>? GetItemsByName(string name)
        {
            List<ItemResponse>? itemsList = null;
            var repoList = _repo.ListItems();
            if (!repoList.IsNullOrEmpty())
            {
                itemsList = repoList!.Where(item => item.Name == name).ToList();
            }

            return itemsList;
        }

        public List<ItemResponse>? ListItems()
        {
            List<ItemResponse>? itemsList = null;
            var repoList = _repo.ListItems();
            if (!repoList.IsNullOrEmpty())            
                itemsList = repoList!;
            
            return itemsList;
        }

        public ItemResponse? UpdateItem(BaseItemRequest updatedItem)
        {
            /*var errors = _validator.Validate(updatedItem);

            if (errors.Any())
                throw new BadRequestException(errors);*/
            
            var mappedItem = _repo.UpdateItem(updatedItem, updatedItem.Id);
            return mappedItem;
        }
    }
}
