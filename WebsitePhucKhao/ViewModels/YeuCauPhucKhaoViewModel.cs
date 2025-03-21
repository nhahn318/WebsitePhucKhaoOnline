using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebsitePhucKhao.ViewModels {
    public class YeuCauPhucKhaoViewModel {
        [Required(ErrorMessage = "Vui lòng nhập mã số sinh viên")]
        public int MaSinhVien { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string HoTen { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string? SoDienThoai { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lớp")]
        public string Lop { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập điểm hiện tại")]
        public float DiemHienTai { get; set; }

        public float DiemMongMuon { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập năm học")]
        public string? NamHoc { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày thi")]
        public DateTime NgayThi { get; set; }

        public string? CaThi { get; set; }
        public string? DiaDiemThi { get; set; }
        public string? PhongThi { get; set; }
        public int? MaGiangVien { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lý do phúc khảo")]
        public string? LyDo { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn môn học")]
        public int? MaMonHoc { get; set; }
        public SelectList? DanhSachMonHoc { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn học kỳ")]
        public int? MaHocKy { get; set; }
        public SelectList? DanhSachHocKy { get; set; }
    }
}
