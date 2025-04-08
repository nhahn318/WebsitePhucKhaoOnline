using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public interface IChuyenNganhRepository {
        Task<IEnumerable<ChuyenNganh>> GetAllAsync();

        Task<ChuyenNganh> GetByIdAsync(int id);
        Task AddAsync(ChuyenNganh chuyenNganh);
        Task UpdateAsync(ChuyenNganh chuyenNganh);
        Task DeleteAsync(int id);


        Task<IEnumerable<ChuyenNganh>> GetByKhoaIdAsync(int maKhoa);

    }
}
