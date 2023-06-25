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
    [Route("api/pallet")]
    [ApiController]
    public class PalletController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public PalletController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/pallet
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pallet>>> GetPallets()
        {
          if (_context.Pallets == null)
          {
              return NotFound();
          }
            return await _context.Pallets.ToListAsync();
        }

        // GET: api/pallet/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pallet>> GetPallet(long id)
        {
          if (_context.Pallets == null)
          {
              return NotFound();
          }
            var pallet = await _context.Pallets.FindAsync(id);

            if (pallet == null)
            {
                return NotFound();
            }

            return pallet;
        }

        // PUT: api/pallet/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPallet(long id, Pallet pallet)
        {
            if (id != pallet.Id)
            {
                return BadRequest();
            }

            _context.Entry(pallet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PalletExists(id))
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

        // POST: api/pallet
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pallet>> PostPallet(Pallet pallet)
        {
          if (_context.Pallets == null)
          {
              return Problem("Entity set 'ApplicationContext.Pallets'  is null.");
          }
            _context.Pallets.Add(pallet);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPallet), new { id = pallet.Id }, pallet);
        }

        // DELETE: api/pallet/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePallet(long id)
        {
            if (_context.Pallets == null)
            {
                return NotFound();
            }
            var pallet = await _context.Pallets.FindAsync(id);
            if (pallet == null)
            {
                return NotFound();
            }

            _context.Pallets.Remove(pallet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PalletExists(long id)
        {
            return (_context.Pallets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
