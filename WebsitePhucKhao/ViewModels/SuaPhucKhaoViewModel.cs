using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebsitePhucKhao.ViewModels {
    public class SuaPhucKhaoViewModel {
        public int MaDon { get; set; }

        public float DiemHienTai { get; set; }

        public float DiemMongMuon { get; set; }
        [Display(Name = "Môn học")]
        public int? MaMonHoc { get; set; }
        [Display(Name = "Học kỳ")]
        public int? MaHocKy { get; set; }
        [Display(Name = "Năm học")]
        public int? MaNamHoc { get; set; }

        public string? DiaDiemThi { get; set; }
        public string? PhongThi { get; set; }

        public string? LyDo { get; set; }

        public SelectList? DanhSachMonHoc { get; set; }
        public SelectList? DanhSachHocKy { get; set; }
        public SelectList? DanhSachNamHoc { get; set; }
    }
}
