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
    [Route("api/palletbay")]
    [ApiController]
    public class PalletBayController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public PalletBayController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/palletbay
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PalletBay>>> GetPalletBays()
        {
          if (_context.PalletBays == null)
          {
              return NotFound();
          }
            return await _context.PalletBays
            .Include(palletBay => palletBay.Pallets)
            .ThenInclude(pallet => pallet.PurchaseOrderItems)
            .ThenInclude(poi => poi.Item)
            .ToListAsync();
        }

        // GET: api/palletbay/contents
        [HttpGet("/api/palletbay/contents/{id}")]
        public async Task<ActionResult<IEnumerable<PalletBayContentsDto>>> GetPalletBayContents(long id)
        {
            if (_context.PalletBays == null)
            {
                return NotFound();
            }

            var palletBay = await _context.PalletBays
                            .Include(pb => pb.Pallets)
                                .ThenInclude(p => p.PurchaseOrderItems)
                                .ThenInclude(poi => poi.Item)
                            .FirstOrDefaultAsync(pb => pb.Id == id);

            if (palletBay == null) 
            {
                return NotFound();
            }

            var contents = new List<PalletBayContentsDto>();

            // Using LINQ
            var filteredContents = from p in palletBay.Pallets
                                   where p.PurchaseOrderItems.Count > 0
                                   where p.PurchaseOrderItems.First().CurrentQuantity > 0
                                   select p;

            // Using LINQ in a different way
            var otherFilteredContents = palletBay.Pallets.Where(p => p.PurchaseOrderItems.Count > 0 && p.PurchaseOrderItems.First().CurrentQuantity > 0)
                                                         .Select(p => p);

            foreach (var pallet in filteredContents) 
            {
                var content = new PalletBayContentsDto 
                {
                    Name = pallet.PurchaseOrderItems.First().Item.Name,
                    PalletId = pallet.Id,
                    PalletBayId = palletBay.Id,
                    Quantity = pallet.PurchaseOrderItems.First().CurrentQuantity
                };
                contents.Add(content);
            }

            return contents;
        }

        // GET: api/palletbay/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PalletBay>> GetPalletBay(long id)
        {
          if (_context.PalletBays == null)
          {
              return NotFound();
          }
            var palletBay = await _context.PalletBays.FindAsync(id);

            if (palletBay == null)
            {
                return NotFound();
            }

            return palletBay;
        }

        // PUT: api/palletbay/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPalletBay(long id, PalletBayUpdateDto palletBayDto)
        {
            if (id != palletBayDto.Id)
            {
                return BadRequest();
            }

            var palletBay = await _context.PalletBays.FindAsync(id);
            if (palletBay == null)
            {
                return NotFound();
            }

            palletBay.Row = palletBayDto.Row;
            palletBay.Section = palletBayDto.Section;
            palletBay.Floor = palletBayDto.Floor;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PalletBayExists(id))
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

        // POST: api/palletbay
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PalletBay>> PostPalletBay(PalletBayCreateDto palletBayDto)
        {
            if (_context.PalletBays == null)
            {
                return Problem("Entity set 'ApplicationContext.PalletBays' is null.");
            }

            var palletBay = new PalletBay
            {
                Row = palletBayDto.Row,
                Section = palletBayDto.Section,
                Floor = palletBayDto.Floor,
                Pallets = new List<Pallet>()
            };

            _context.PalletBays.Add(palletBay);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPalletBay), new { id = palletBay.Id }, palletBay);
        }

        // POST: api/palletbay/bulk
        [HttpPost("bulk")]
        public async Task<ActionResult<PalletBay>> PostPalletBayBulk(PalletBayCreateBulkDto palletBayDto)
        {
            if (_context.PalletBays == null)
            {
                return Problem("Entity set 'ApplicationContext.PalletBays' is null.");
            }

            if (palletBayDto.Row == null || 
                palletBayDto.Row == "" || 
                palletBayDto.StartFloor <= 0 || 
                palletBayDto.EndFloor < palletBayDto.StartFloor || 
                palletBayDto.StartSection <= 0 || 
                palletBayDto.EndSection < palletBayDto.StartSection)
            {
                return BadRequest();
            }

            for(var f = palletBayDto.StartFloor; f <= palletBayDto.EndFloor; f++)
            {
                for (var s = palletBayDto.StartSection; s <= palletBayDto.EndSection; s++) 
                {
                    // TODO: Update pallet bay model so that sections and floors are numbers, not strings
                    var palletBay = new PalletBay
                    {
                        Row = palletBayDto.Row,
                        Section = s.ToString(),
                        Floor = f.ToString(),
                        Pallets = new List<Pallet>()
                    };
                    _context.PalletBays.Add(palletBay);
                }
            }
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/palletbay/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePalletBay(long id)
        {
            if (_context.PalletBays == null)
            {
                return NotFound();
            }
            var palletBay = await _context.PalletBays.FindAsync(id);
            if (palletBay == null)
            {
                return NotFound();
            }

            _context.PalletBays.Remove(palletBay);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PalletBayExists(long id)
        {
            return (_context.PalletBays?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
