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
    [Route("api/salesorderitem")]
    [ApiController]
    public class SalesOrderItemController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public SalesOrderItemController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/salesorderitem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesOrderItem>>> GetSalesOrderItems()
        {
          if (_context.SalesOrderItems == null)
          {
              return NotFound();
          }
            return await _context.SalesOrderItems.ToListAsync();
        }

        // GET: api/salesorderitem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesOrderItem>> GetSalesOrderItem(long id)
        {
          if (_context.SalesOrderItems == null)
          {
              return NotFound();
          }
            var salesOrderItem = await _context.SalesOrderItems.FindAsync(id);

            if (salesOrderItem == null)
            {
                return NotFound();
            }

            return salesOrderItem;
        }

        // PUT: api/salesorderitem/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalesOrderItem(long id, SalesOrderItemUpdateDto salesOrderItemDto)
        {
            if (id != salesOrderItemDto.Id)
            {
                return BadRequest();
            }

            var salesOrderItem = await _context.SalesOrderItems.FindAsync(salesOrderItemDto.Id);
            if (salesOrderItem == null)
            {
                return NotFound();
            }

            salesOrderItem.Quantity = salesOrderItemDto.Quantity;
            salesOrderItem.UnitPrice = salesOrderItemDto.UnitPrice;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesOrderItemExists(id))
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

        // POST: api/salesorderitem
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SalesOrderItem>> PostSalesOrderItem(SalesOrderItemCreateDto salesOrderItemDto)
        {
            if (_context.SalesOrderItems == null)
            {
                return Problem("Entity set 'ApplicationContext.SalesOrderItems'  is null.");
            }

            var purchaseOrderItem = await _context.PurchaseOrderItems.FindAsync(salesOrderItemDto.PurchaseOrderItemId);
            if (purchaseOrderItem == null)
            {
                return BadRequest();
            }

            var salesOrder = await _context.SalesOrders.FindAsync(salesOrderItemDto.SalesOrderId);
            if (salesOrder == null)
            {
                return BadRequest();
            }

            var salesOrderItem = new SalesOrderItem
            {
                SalesOrder = salesOrder,
                SalesOrderId = salesOrderItemDto.SalesOrderId,
                PurchaseOrderItem = purchaseOrderItem,
                PurchaseOrderItemId = salesOrderItemDto.PurchaseOrderItemId,
                Quantity = salesOrderItemDto.Quantity,
                UnitPrice = salesOrderItemDto.UnitPrice
            };

            _context.SalesOrderItems.Add(salesOrderItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSalesOrderItem), new { id = salesOrderItem.Id }, salesOrderItem);
        }

        // DELETE: api/salesorderitem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesOrderItem(long id)
        {
            if (_context.SalesOrderItems == null)
            {
                return NotFound();
            }
            var salesOrderItem = await _context.SalesOrderItems.FindAsync(id);
            if (salesOrderItem == null)
            {
                return NotFound();
            }

            _context.SalesOrderItems.Remove(salesOrderItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalesOrderItemExists(long id)
        {
            return (_context.SalesOrderItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
