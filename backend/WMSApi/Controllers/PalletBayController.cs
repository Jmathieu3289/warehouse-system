using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WMSApi.Models;
using WMSApi.Services;

namespace WMSApi.Controllers
{
    [Route("api/palletbay")]
    [ApiController]
    public class PalletBayController : ControllerBase
    {
        private readonly IPalletBayService _service;

        public PalletBayController(IPalletBayService service)
        {
            _service = service;
        }

        // GET: api/palletbay
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PalletBay>>> GetPalletBays()
        {
            var palletBays = await _service.GetPalletBays();
            if (palletBays == null)
            {
                return NotFound();
            }
            return Ok(palletBays);
        }

        // GET: api/palletbay/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PalletBay>> GetPalletBay(long id)
        {
            var palletBay = await _service.GetPalletBayById(id);
            if (palletBay == null)
            {
                return NotFound();
            }
            return palletBay;
        }

        // PUT: api/palletbay/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPalletBay(long id, PalletBayUpdateDto palletBayDto)
        {
            if (id != palletBayDto.Id)
            {
                return BadRequest();
            }

            try 
            {
                var palletBay = await _service.UpdatePalletBay(id, palletBayDto);
                if (palletBay == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_service.PalletBayExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/palletbay
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PalletBay>> PostPalletBay(PalletBayCreateDto palletBayDto)
        {
            var palletBay = await _service.CreatePalletBay(palletBayDto);
            if (palletBay == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetPalletBay), new { id = palletBay.Id }, palletBay);
        }

        //POST: api/palletbay/bulk
        [HttpPost("bulk")]
        public async Task<ActionResult<IEnumerable<PalletBay>>> PostPalletBayBulk(PalletBayCreateBulkDto dto)
        {
            var palletBays = await _service.CreatePalletBayBulk(dto);
            if (palletBays == null)
            {
                return BadRequest();
            }
            return Ok(palletBays);
        }

        // DELETE: api/palletbay/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePalletBay(long id)
        {
            var palletBay = await _service.DeletePalletBay(id);
            if (palletBay == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
