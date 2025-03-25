
using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.ViewModels {
    public class ChiTietUploadViewModel {
        public DonPhucKhao? Don { get; set; }
        public List<HinhAnhBaiThi>? DanhSachAnh { get; set; }
        public DonPhucKhaoChiTiet? ChiTiet { get; set; }
    }
}
