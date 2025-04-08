using Microsoft.AspNetCore.Mvc.Rendering;
using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.ViewModels {
    public class EditUploadViewModel {
        public int MaDon { get; set; }
        public long? MaGiangVien { get; set; }

        public List<HinhAnhBaiThi>? DanhSachAnh { get; set; }

        public SelectList? DanhSachGiangVien { get; set; }

        public List<int> AnhCanXoa { get; set; } = new(); // danh sách ID hình ảnh cần xóa

        public List<IFormFile> AnhMoi { get; set; } = new(); // ảnh upload mới
    }

}
