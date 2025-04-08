using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories {
    public interface IKhoaRepository {
        Task<IEnumerable<Khoa>> GetAllAsync();
        Task<Khoa?> GetByIdAsync(int id);
        Task AddAsync(Khoa khoa);
        Task UpdateAsync(Khoa khoa);
        Task DeleteAsync(int id);
    }
}
