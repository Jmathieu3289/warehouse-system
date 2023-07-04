using Microsoft.EntityFrameworkCore;
using WMSApi.Models;

namespace WMSApi.Services;

public class PalletService : IPalletService
{
    private readonly ApplicationContext _context;

    public PalletService(ApplicationContext context) 
    {
        _context = context; 
    }

    public async Task<Pallet?> CreatePallet(PalletCreateDto dto)
    {
        var palletBay = await _context.PalletBays.FindAsync(dto.PalletBayId);
        if (palletBay == null)
        {
            return null;
        }

        var pallet = new Pallet
        {
            PalletBayId = dto.PalletBayId,
            PalletBay = palletBay,
            PurchaseOrderItems = new List<PurchaseOrderItem>()
        };

        _context.Pallets.Add(pallet);
        await _context.SaveChangesAsync();
        return pallet;
    }

    public async Task<Pallet?> DeletePallet(long id)
    {
        var pallet = await _context.Pallets.FindAsync(id);
        if (pallet == null)
        {
            return null;
        }

        _context.Pallets.Remove(pallet);
        await _context.SaveChangesAsync(); 
        return pallet;
    }

    public async Task<Pallet?> GetPalletById(long id)
    {
        return await _context.Pallets.FindAsync(id);
    }

    public async Task<IEnumerable<Pallet>?> GetPallets()
    {
        return await _context.Pallets.ToListAsync();
    }

    public bool PalletExists(long id)
    {
        return (_context.Pallets?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    public async Task<Pallet?> UpdatePallet(long id, PalletUpdateDto dto)
    {
        var pallet = await _context.Pallets.FindAsync(id);
        if (pallet == null)
        {
            return null;
        }

        // Update

        await _context.SaveChangesAsync();

        return pallet;
    }
}
