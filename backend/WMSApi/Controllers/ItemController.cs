using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMSApi.Models;
using WMSApi.Services;

namespace WMSApi.Controllers
{
    [Route("api/item")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _service;

        public ItemController(IItemService service)
        {
            _service = service;
        }

        // GET: api/item
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            var items = await _service.GetItems();
            if (items == null)
            {
                return NotFound();
            }
            return Ok(items);
        }

        // GET: api/item/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(long id)
        {
            var item = await _service.GetItemById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        // PUT: api/item/5
        // To protect from overiteming attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(long id, ItemUpdateDto itemDto)
        {
            if (id != itemDto.Id)
            {
                return BadRequest();
            }

            try 
            {
                var item = await _service.UpdateItem(id, itemDto);
                if (item == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_service.ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/item
        // To protect from overiteming attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(ItemCreateDto itemDto)
        {
            var item = await _service.CreateItem(itemDto);
            if (item == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        }

        // DELETE: api/item/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(long id)
        {
            var item = await _service.DeleteItem(id);
            if (item == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
