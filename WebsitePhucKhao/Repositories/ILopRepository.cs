using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public interface ILopRepository {
        Task<IEnumerable<Lop>> GetAllAsync();
        Task<Lop?> GetByIdAsync(int maLop);
        Task AddAsync(Lop lop);
        Task UpdateAsync(Lop lop);
        Task DeleteAsync(int maLop);
        Task<IEnumerable<Lop>> GetByKhoaIdAsync(int maKhoa);
    }
}
