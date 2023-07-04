using WMSApi.Models;

namespace WMSApi.Services;

public interface IPalletService
{
    Task<IEnumerable<Pallet>?> GetPallets();
    Task<Pallet?> GetPalletById(long id);
    Task<Pallet?> CreatePallet(PalletCreateDto dto);
    Task<Pallet?> UpdatePallet(long id, PalletUpdateDto dto);
    Task<Pallet?> DeletePallet(long id);
    bool PalletExists(long id);
}
