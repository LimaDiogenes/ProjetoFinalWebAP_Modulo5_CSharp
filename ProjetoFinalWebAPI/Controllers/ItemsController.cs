using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Requests;
using Services;

namespace Controllers
{
    [Route("GridironStore/Items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService itemService;
        public ItemsController(IItemService service)
        {
            itemService = service;
        }

        [HttpGet("ListAllItems")]
        public IActionResult Get()
        {
            return Ok(itemService.ListItems());
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


    }
}
