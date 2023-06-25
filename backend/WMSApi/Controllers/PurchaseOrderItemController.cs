using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMSApi.Models;

namespace WMSApi.Controllers
{
    [Route("api/purchaseorderitem")]
    [ApiController]
    public class PurchaseOrderItemController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public PurchaseOrderItemController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/purchaseorderitem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseOrderItem>>> GetPurchaseOrderItems()
        {
          if (_context.PurchaseOrderItems == null)
          {
              return NotFound();
          }
            return await _context.PurchaseOrderItems.ToListAsync();
        }

        // GET: api//purchaseorderitem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseOrderItem>> GetPurchaseOrderItem(long id)
        {
          if (_context.PurchaseOrderItems == null)
          {
              return NotFound();
          }
            var purchaseOrderItem = await _context.PurchaseOrderItems.FindAsync(id);

            if (purchaseOrderItem == null)
            {
                return NotFound();
            }

            return purchaseOrderItem;
        }

        // PUT: api/purchaseorderitem/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchaseOrderItem(long id, PurchaseOrderItem purchaseOrderItem)
        {
            if (id != purchaseOrderItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(purchaseOrderItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseOrderItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/purchaseorderitem
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PurchaseOrderItem>> PostPurchaseOrderItem(PurchaseOrderItem purchaseOrderItem)
        {
          if (_context.PurchaseOrderItems == null)
          {
              return Problem("Entity set 'ApplicationContext.PurchaseOrderItems'  is null.");
          }
            _context.PurchaseOrderItems.Add(purchaseOrderItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPurchaseOrderItem), new { id = purchaseOrderItem.Id }, purchaseOrderItem);
        }

        // DELETE: api/purchaseorderitem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaseOrderItem(long id)
        {
            if (_context.PurchaseOrderItems == null)
            {
                return NotFound();
            }
            var purchaseOrderItem = await _context.PurchaseOrderItems.FindAsync(id);
            if (purchaseOrderItem == null)
            {
                return NotFound();
            }

            _context.PurchaseOrderItems.Remove(purchaseOrderItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PurchaseOrderItemExists(long id)
        {
            return (_context.PurchaseOrderItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
