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
        public async Task<IActionResult> PutSalesOrder(long id, SalesOrder salesOrder)
        {
            if (id != salesOrder.Id)
            {
                return BadRequest();
            }

            _context.Entry(salesOrder).State = EntityState.Modified;

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
        public async Task<ActionResult<SalesOrder>> PostSalesOrder(SalesOrder salesOrder)
        {
          if (_context.SalesOrders == null)
          {
              return Problem("Entity set 'ApplicationContext.SalesOrders'  is null.");
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
