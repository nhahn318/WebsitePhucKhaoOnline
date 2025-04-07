using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public interface IDonPhucKhaoChiTietRepository {
        Task<List<DonPhucKhaoChiTiet>> GetPhucKhaoByGiangVienAsync(long maGiangVien);
        Task<List<DonPhucKhaoChiTiet>> GetDaChamByGiangVienAsync(long maGiangVien);
    }
}
