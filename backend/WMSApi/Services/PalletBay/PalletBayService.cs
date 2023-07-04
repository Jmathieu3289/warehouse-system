using Microsoft.EntityFrameworkCore;
using WMSApi.Models;

namespace WMSApi.Services;

public class PalletBayService : IPalletBayService
{
    private readonly ApplicationContext _context;

    public PalletBayService(ApplicationContext context) 
    {
        _context = context; 
    }

    public async Task<PalletBay?> CreatePalletBay(PalletBayCreateDto dto)
    {
        var palletBay = new PalletBay
        {
            Row = dto.Row,
            Section = dto.Section,
            Floor = dto.Floor,
            Pallets = new List<Pallet>()
        };

        _context.PalletBays.Add(palletBay);
        await _context.SaveChangesAsync();
        return palletBay;
    }

    public async Task<PalletBay?> DeletePalletBay(long id)
    {
        var palletBay = await _context.PalletBays.FindAsync(id);
        if (palletBay == null)
        {
            return null;
        }

        _context.PalletBays.Remove(palletBay);
        await _context.SaveChangesAsync(); 
        return palletBay;
    }

    public async Task<PalletBay?> GetPalletBayById(long id)
    {
        return await _context.PalletBays
            .Include(pb => pb.Pallets)
                .ThenInclude(p => p.PurchaseOrderItems)
                .ThenInclude(poi => poi.Item)
            .FirstOrDefaultAsync(pb => pb.Id == id);
    }

    public async Task<IEnumerable<PalletBay>?> GetPalletBays()
    {
        return await _context.PalletBays
            .Include(palletBay => palletBay.Pallets)
                .ThenInclude(pallet => pallet.PurchaseOrderItems)
                .ThenInclude(poi => poi.Item)
            .ToListAsync();
    }

    public bool PalletBayExists(long id)
    {
        return (_context.PalletBays?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    public async Task<PalletBay?> UpdatePalletBay(long id, PalletBayUpdateDto dto)
    {
        var palletBay = await _context.PalletBays.FindAsync(id);
        if (palletBay == null)
        {
            return null;
        }

        palletBay.Row = dto.Row;
        palletBay.Section = dto.Section;
        palletBay.Floor = dto.Floor;

        await _context.SaveChangesAsync();

        return palletBay;
    }

    public async Task<IEnumerable<PalletBay>?> CreatePalletBayBulk(PalletBayCreateBulkDto dto)
    {
        if (dto.Row == null || 
                dto.Row == "" || 
                dto.StartFloor <= 0 || 
                dto.EndFloor < dto.StartFloor || 
                dto.StartSection <= 0 || 
                dto.EndSection < dto.StartSection)
        {
            return null;
        }

        var palletBays = new List<PalletBay>();

        for(var f = dto.StartFloor; f <= dto.EndFloor; f++)
        {
            for (var s = dto.StartSection; s <= dto.EndSection; s++) 
            {
                // TODO: Update pallet bay model so that sections and floors are numbers, not strings
                var palletBay = new PalletBay
                {
                    Row = dto.Row,
                    Section = s.ToString(),
                    Floor = f.ToString(),
                    Pallets = new List<Pallet>()
                };
                _context.PalletBays.Add(palletBay);
                palletBays.Add(palletBay);
            }
        }
            
        await _context.SaveChangesAsync();
        return palletBays;
    }
}
