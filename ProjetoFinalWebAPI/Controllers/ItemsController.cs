﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Requests;
using Services;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace Controllers
{
    [Route("GridironStore/Items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService itemService;
        private readonly IUserService userService;
        public ItemsController(IItemService service, IUserService userService)
        {
            itemService = service;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Get(string? searchAll,
                                 string? searchCategory,
                                 string? searchName)
        {
            List<ItemResponse>? itemsList = itemService.ListItems();
            if (itemsList != null)
                itemsList = itemsList.OrderBy(i => i.Name).ToList();
            List<List<ItemResponse>> itemsResponse = new();

            string? _searchAll = searchAll;
            string? _searchCategory = searchCategory;
            string? _searchName = searchName;

            TextInfo myTI = CultureInfo.InvariantCulture.TextInfo;

            if (searchAll != null)
                _searchAll = myTI.ToTitleCase(searchAll!).Trim();
            if (searchCategory != null)
                _searchCategory = myTI.ToTitleCase(searchCategory!).Trim();
            if (searchName != null)
                _searchName = myTI.ToTitleCase(searchName!).Trim();

            if (_searchAll.IsNullOrEmpty() && _searchCategory.IsNullOrEmpty() && _searchName.IsNullOrEmpty())
            {
                return Ok(itemsList.IsNullOrEmpty() ? NotFound("No items in database") : new
                {
                    message = "List of all items:",
                    categories = itemsList!.Select(item => item.Category).Distinct(),
                    items = itemsList
                });
            }

            if (!_searchAll.IsNullOrEmpty())
            {
                itemsResponse.Add(itemService.GetItemsByAnyField(_searchAll!)!);
            }

            if (!_searchCategory.IsNullOrEmpty())
            {
                if (itemsResponse.Any())
                {
                    foreach (var list in itemsResponse)
                    {
                        var newList = list.Where(item => item.Category.Contains(_searchCategory!)).ToList();
                        list.Clear();
                        list.AddRange(newList);
                    }
                }
                else
                    itemsResponse.Add(itemService.GetItemsByCategory(_searchCategory!)!);
            }

            if (!_searchName.IsNullOrEmpty())
            {
                if (itemsResponse.Any())
                {
                    foreach (var list in itemsResponse)
                    {
                        var newList = list.Where(item => item.Name.Contains(_searchName!)).ToList();
                        list.Clear();
                        list.AddRange(newList);
                    }
                }
                else
                    itemsResponse.Add(itemService.GetItemsByName(_searchName!)!);
            }

            // return when one or more items are found by search
            if (itemsResponse.All(innerList => innerList.Any()))
                return Ok(itemsResponse.Distinct());

            // return when no match was found by search
            return Ok(new
            {
                message = "No items found by search",
                list = "List of all items:",
                categories = itemsList!.Select(item => item.Category).Distinct(),
                items = itemsList
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromForm] int itemId)
        {
            var user = GetUserFromJwt();
            var cartService = new CartService(user);
            var item = itemService.GetItemById(itemId);
            var resultList = cartService.AddToCart(item);

            return Ok(resultList);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateItem")]
        public IActionResult Post([FromForm] BaseItemRequest item)
        {
            return Ok(itemService.CreateItem(item));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteItem")]
        public IActionResult Delete([FromForm] int id)
        {
            var itemToDelete = itemService.GetItemById(id);
            if (itemToDelete != null) { itemService.DeleteItem(id); }
            return itemToDelete == null ? NotFound("Item not found in database") : Ok($"Item deleteded succesfully: \n " +
                                                                                      $"Name: {itemToDelete.Name} \n" +
                                                                                      $"Variant: {itemToDelete.Variant} \n" +
                                                                                      $"Size: {itemToDelete.Size}");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateItem/{id}")]
        public IActionResult Put(int id, [FromForm] BaseItemRequest item)
        {
            var itemToUpdate = itemService.GetItemById(id);
            if (itemToUpdate != null)
            {
                item.Id = id;
                var updatedItem = itemService.UpdateItem(item);
                return Ok($"Item updated succesfully: \n\n " +
                          $"Name: {updatedItem!.Name} \n" +
                          $" Category: {updatedItem.Category} \n" +
                          $" Variant: {updatedItem.Variant} \n" +
                          $" Size: {updatedItem.Size} \n" +
                          $" Price: {updatedItem.Price}");
            };

            return NotFound("Item not found in database");
        }

        private UserResponse GetUserFromJwt()
        {            
            int userID;
            try
            {
                userID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            }
            catch (System.Exception)
            {
                throw new Exceptions.NotFoundException("Invalid token information - User not found");
            }

            return userService.GetUserById(userID)!;
        }
    }
}
