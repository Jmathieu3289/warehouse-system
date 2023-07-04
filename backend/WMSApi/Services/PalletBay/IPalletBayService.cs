using WMSApi.Models;

namespace WMSApi.Services;

public interface IPalletBayService
{
    Task<IEnumerable<PalletBay>?> GetPalletBays();
    Task<PalletBay?> GetPalletBayById(long id);
    Task<PalletBay?> CreatePalletBay(PalletBayCreateDto dto);
    Task<IEnumerable<PalletBay>?> CreatePalletBayBulk(PalletBayCreateBulkDto dto);
    Task<PalletBay?> UpdatePalletBay(long id, PalletBayUpdateDto dto);
    Task<PalletBay?> DeletePalletBay(long id);
    bool PalletBayExists(long id);
}
