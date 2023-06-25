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
    [Route("api/salesorder")]
    [ApiController]
    public class SalesOrderController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public SalesOrderController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/salesorder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesOrder>>> GetSalesOrders()
        {
          if (_context.SalesOrders == null)
          {
              return NotFound();
          }
            return await _context.SalesOrders.Include(salesorder => salesorder.SalesOrderItems).ToListAsync();
        }

        // GET: api/salesorder/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesOrder>> GetSalesOrder(long id)
        {
          if (_context.SalesOrders == null)
          {
              return NotFound();
          }
            var salesOrder = await _context.SalesOrders.FindAsync(id);

            if (salesOrder == null)
            {
                return NotFound();
            }

            return salesOrder;
        }

        // PUT: api/salesorder/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalesOrder(long id, SalesOrderUpdateDto salesOrderDto)
        {
            if (id != salesOrderDto.Id)
            {
                return BadRequest();
            }

            var salesOrder = await _context.SalesOrders.FindAsync(id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            switch (salesOrderDto.Status) {
                case SalesOrderStatus.Filling:
                    salesOrder.DateFilling = DateTime.Now;
                    break;
                case SalesOrderStatus.Staged:
                    salesOrder.DateStaged = DateTime.Now;
                    break;
                case SalesOrderStatus.InTransit:
                    salesOrder.DateShipped = DateTime.Now;
                    break;
                case SalesOrderStatus.Closed:
                    salesOrder.DateReceived = DateTime.Now;
                    break;
            }
            salesOrder.Status = salesOrderDto.Status;
            salesOrder.Comments = salesOrderDto.Comments;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesOrderExists(id))
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

        // POST: api/salesorder
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SalesOrder>> PostSalesOrder(SalesOrderCreateDto salesOrderDto)
        {
            if (_context.SalesOrders == null)
            {
                return Problem("Entity set 'ApplicationContext.SalesOrders'  is null.");
            }

            if (salesOrderDto.SalesOrderItems == null || salesOrderDto.SalesOrderItems.Count == 0)
            {
                return BadRequest("At least one Sales Order Item is required.");
            }

            // Create SalesOrder from DTO
            var salesOrder = new SalesOrder
            {
                Status = SalesOrderStatus.Submitted,
                DateCreated = DateTime.Now,
                DateLastModified = DateTime.Now,
                Comments = salesOrderDto.Comments,
                SalesOrderItems = new List<SalesOrderItem>()
            };

            // Set ID
            _context.SalesOrders.Add(salesOrder);

            // Add SalesOrderItems from DTO
            foreach(var soItemDto in salesOrderDto.SalesOrderItems)
            {
                var purchaseOrderItem = await _context.PurchaseOrderItems.FindAsync(soItemDto.PurchaseOrderItemId);
                if (purchaseOrderItem == null)
                {
                    return BadRequest();
                }

                var soItem = new SalesOrderItem
                {
                    PurchaseOrderItem = purchaseOrderItem,
                    PurchaseOrderItemId = soItemDto.PurchaseOrderItemId,
                    SalesOrder = salesOrder,
                    Quantity = soItemDto.Quantity,
                    UnitPrice = soItemDto.UnitPrice
                };

                salesOrder.SalesOrderItems.Add(soItem);
            }

            _context.SalesOrders.Add(salesOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSalesOrder), new { id = salesOrder.Id }, salesOrder);
        }

        // DELETE: api/salesorder/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalesOrder(long id)
        {
            if (_context.SalesOrders == null)
            {
                return NotFound();
            }
            var salesOrder = await _context.SalesOrders.FindAsync(id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            _context.SalesOrders.Remove(salesOrder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalesOrderExists(long id)
        {
            return (_context.SalesOrders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
