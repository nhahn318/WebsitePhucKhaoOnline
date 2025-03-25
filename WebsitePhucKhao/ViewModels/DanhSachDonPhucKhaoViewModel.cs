using WebsitePhucKhao.Enums;

namespace WebsitePhucKhao.ViewModels {
    public class DanhSachDonPhucKhaoViewModel {
        public int STT { get; set; }
        public int MaDon { get; set; }
        public DateTime NgayGui { get; set; }
        public TrangThaiPhucKhao TrangThai { get; set; }
    }
}
