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
    [Route("api/purchaseorder")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public PurchaseOrderController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/purchaseorder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseOrder>>> GetPurchaseOrders()
        {
          if (_context.PurchaseOrders == null)
          {
              return NotFound();
          }
            return await _context.PurchaseOrders.ToListAsync();
        }

        // GET: api/purchaseorder/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PurchaseOrder>> GetPurchaseOrder(long id)
        {
          if (_context.PurchaseOrders == null)
          {
              return NotFound();
          }
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return purchaseOrder;
        }

        // PUT: api/purchaseorder/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchaseOrder(long id, PurchaseOrderUpdateDto purchaseOrderDto)
        {
            if (id != purchaseOrderDto.Id)
            {
                return BadRequest();
            }

            // Find the existing Purchase Order
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            purchaseOrder.Status = purchaseOrderDto.Status;
            purchaseOrder.DateEstimatedDelivery = purchaseOrderDto.DateEstimatedDelivery;
            purchaseOrder.DateReceived = purchaseOrderDto.DateReceived;
            purchaseOrder.DateLastModified = DateTime.Now;
            purchaseOrder.Comments = purchaseOrderDto.Comments;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseOrderExists(id))
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

        // POST: api/purchaseorder
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PurchaseOrder>> PostPurchaseOrder(PurchaseOrderCreateDto purchaseOrderDto)
        {
            if (_context.PurchaseOrders == null)
            {
                return Problem("Entity set 'ApplicationContext.PurchaseOrders' is null.");
            }

            if (purchaseOrderDto.PurchaseOrderItems == null || purchaseOrderDto.PurchaseOrderItems.Count == 0)
            {
                return BadRequest("At least one Purchase Order Item is required.");
            }

            // Create PurchaseOrder from DTO
            var purchaseOrder = new PurchaseOrder
            {
                Status = PurchaseOrderStatus.Submitted,
                DateCreated = DateTime.Now,
                DateEstimatedDelivery = purchaseOrderDto.DateEstimatedDelivery,
                DateLastModified = DateTime.Now,
                Comments = purchaseOrderDto.Comments,
                PurchaseOrderItems = new List<PurchaseOrderItem>()
            };

            // Set ID
            _context.PurchaseOrders.Add(purchaseOrder);

            // Add PurchaseOrderItems from DTO
            foreach(var poItemDto in purchaseOrderDto.PurchaseOrderItems)
            {
                var item = await _context.Items.FindAsync(poItemDto.ItemId);
                if (item == null)
                {
                    return BadRequest();
                }

                var poItem = new PurchaseOrderItem
                {
                    ItemId = poItemDto.ItemId,
                    Item = item,
                    PurchaseOrder = purchaseOrder,
                    PurchasedQuantity = poItemDto.PurchasedQuantity,
                    CurrentQuantity = poItemDto.PurchasedQuantity,
                    Weight = poItemDto.Weight,
                    UnitPrice = poItemDto.UnitPrice,
                    MarkupPrice = poItemDto.MarkupPrice,
                    MarginPrice = poItemDto.MarginPrice,
                    FreightPrice = poItemDto.FreightPrice,
                    SellPrice = poItemDto.UnitPrice + poItemDto.MarkupPrice + poItemDto.MarginPrice + poItemDto.FreightPrice
                };

                purchaseOrder.PurchaseOrderItems.Add(poItem);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPurchaseOrder", new { id = purchaseOrder.Id }, purchaseOrder);
        }

        // DELETE: api/purchaseorder/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchaseOrder(long id)
        {
            if (_context.PurchaseOrders == null)
            {
                return NotFound();
            }
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            _context.PurchaseOrders.Remove(purchaseOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PurchaseOrderExists(long id)
        {
            return (_context.PurchaseOrders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
