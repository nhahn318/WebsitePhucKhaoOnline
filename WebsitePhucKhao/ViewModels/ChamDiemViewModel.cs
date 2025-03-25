using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.ViewModels {
    public class ChamDiemViewModel {
        public int MaDon { get; set; }
        public string? TenSinhVien { get; set; }
        public string? TenMonHoc { get; set; }
        public double? DiemTruocPhucKhao { get; set; }
        public double? DiemMongMuon { get; set; }
        public string? NhanXet { get; set; }
        public string? LyDo { get; set; }
        public List<HinhAnhBaiThi> DanhSachAnh { get; set; } = new();
        public float? DiemSauPhucKhao { get; set; }
    }
}
