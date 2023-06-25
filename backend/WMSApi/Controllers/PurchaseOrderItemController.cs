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
        public async Task<IActionResult> PutPurchaseOrderItem(long id, PurchaseOrderItemUpdateDto purchaseOrderItemDto)
        {
            if (id != purchaseOrderItemDto.Id)
            {
                return BadRequest();
            }

            var purchaseOrderItem = await _context.PurchaseOrderItems.FindAsync(purchaseOrderItemDto.Id);
            if (purchaseOrderItem == null)
            {
                return NotFound();
            }

            if (purchaseOrderItem.PalletId != purchaseOrderItemDto.PalletId) {
                var pallet = await _context.Pallets.FindAsync(purchaseOrderItemDto.PalletId);
                if (pallet == null)
                {
                    return BadRequest();
                }
                else
                {
                    purchaseOrderItem.Pallet = pallet;
                    purchaseOrderItem.PalletId = purchaseOrderItemDto.PalletId;
                }
            }

            if (purchaseOrderItemDto.PurchasedQuantity < purchaseOrderItem.CurrentQuantity)
            {
                return Problem("Purchased quantity cannot be less than remaining quantity.");
            }

            purchaseOrderItem.PurchasedQuantity = purchaseOrderItemDto.PurchasedQuantity;
            purchaseOrderItem.CurrentQuantity = Math.Min(purchaseOrderItem.CurrentQuantity, purchaseOrderItemDto.PurchasedQuantity);
            purchaseOrderItem.Weight = purchaseOrderItemDto.Weight;
            purchaseOrderItem.UnitPrice = purchaseOrderItemDto.UnitPrice;
            purchaseOrderItem.MarkupPrice = purchaseOrderItemDto.MarkupPrice;
            purchaseOrderItem.MarginPrice = purchaseOrderItemDto.MarginPrice;
            purchaseOrderItem.FreightPrice = purchaseOrderItemDto.FreightPrice;
            purchaseOrderItem.SellPrice = purchaseOrderItemDto.UnitPrice + purchaseOrderItemDto.MarginPrice + purchaseOrderItemDto.MarginPrice + purchaseOrderItemDto.FreightPrice;

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
        public async Task<ActionResult<PurchaseOrderItem>> PostPurchaseOrderItem(PurchaseOrderItemCreateDto purchaseOrderItemDto)
        {
            if (_context.PurchaseOrderItems == null)
            {
                return Problem("Entity set 'ApplicationContext.PurchaseOrderItems' is null.");
            }

            var item = await _context.Items.FindAsync(purchaseOrderItemDto.ItemId);
            if (item == null)
            {
                return BadRequest();
            }

            var purchaseOrder = await _context.PurchaseOrders.FindAsync(purchaseOrderItemDto.PurchaseOrderId);
            if (purchaseOrder == null)
            {
                return BadRequest();
            }

            var purchaseOrderItem = new PurchaseOrderItem
            {
                ItemId = purchaseOrderItemDto.ItemId,
                Item = item,
                PurchaseOrderId = purchaseOrderItemDto.PurchaseOrderId,
                PurchaseOrder = purchaseOrder,
                PurchasedQuantity = purchaseOrderItemDto.PurchasedQuantity,
                Weight = purchaseOrderItemDto.Weight,
                UnitPrice = purchaseOrderItemDto.UnitPrice,
                MarkupPrice = purchaseOrderItemDto.MarkupPrice,
                MarginPrice = purchaseOrderItemDto.MarginPrice,
                FreightPrice = purchaseOrderItemDto.FreightPrice,
                SellPrice = purchaseOrderItemDto.UnitPrice + purchaseOrderItemDto.MarginPrice + purchaseOrderItemDto.MarginPrice + purchaseOrderItemDto.FreightPrice
            };

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
