using WebsitePhucKhao.Enums;
using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.ViewModels {

    public class DonPhucKhaoSauKhiChamViewModel {
        public int STT { get; set; }
        public int MaDon { get; set; }
        public DateTime NgayGui { get; set; }
        public string TrangThai { get; set; }

        public float? DiemHienTai { get; set; }
        public float? DiemSauPhucKhao { get; set; }
        public string? NhanXet { get; set; }
        public string? BaiGiaiTayUrl { get; set; }
        public List<HinhAnhBaiThi> DanhSachAnh { get; set; }
    }
}
