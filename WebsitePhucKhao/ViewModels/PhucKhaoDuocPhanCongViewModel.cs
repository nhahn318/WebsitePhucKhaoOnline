using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.ViewModels {
    public class PhucKhaoDuocPhanCongViewModel {
        public long MaDon { get; set; }
        public string TenSinhVien { get; set; } = string.Empty;
        public SinhVien? SinhVien { get; set; }
        public string TenMonHoc { get; set; } = string.Empty;
        public MonHoc? MonHoc { get; set; }
        public double? DiemTruocPhucKhao { get; set; }
        public double DiemMongMuon { get; set; }
        public string? LyDo { get; set; }
        public DateTime? NgayChamLai { get; set; }
        public string? NguoiDuyet { get; set; }
        public float? DiemSauPhucKhao { get; set; }

    }

}
