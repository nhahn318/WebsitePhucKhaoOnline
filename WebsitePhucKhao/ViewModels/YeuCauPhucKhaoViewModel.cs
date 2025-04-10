using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebsitePhucKhao.ViewModels {
    public class YeuCauPhucKhaoViewModel {
        public int MaDon { get; set; }

        [Display(Name = "Mã sinh viên")]
        [Required(ErrorMessage = "Vui lòng nhập mã số sinh viên")]
        public long MaSinhVien { get; set; }

        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string HoTen { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = null!;


        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string? SoDienThoai { get; set; }

        [Display(Name = "Lớp")]
        [Required(ErrorMessage = "Vui lòng nhập lớp")]
        public string Lop { get; set; } = string.Empty;

        [Display(Name = "Điểm hiện tại")]
        [Required(ErrorMessage = "Vui lòng nhập điểm hiện tại")]
        public float DiemHienTai { get; set; }

        [Display(Name = "Điểm mong muốn")]
        [Required(ErrorMessage = "Vui lòng nhập điểm mong muốn.")]
        [Range(1.0, 10.0, ErrorMessage = "Điểm mong muốn phải là số từ 1 đến 10.")]
        public float DiemMongMuon { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập năm học")]
        [Display(Name = "Năm học")]
        public int? MaNamHoc { get; set; } // ID năm học được chọn
        public SelectList? DanhSachNamHoc { get; set; } // Dropdown năm học



        [Display(Name = "Ngày thi")]
        [Required(ErrorMessage = "Vui lòng nhập ngày thi")]
        public DateTime? NgayThi { get; set; }

        public string? CaThi { get; set; }

        [Display(Name = "Địa điểm thi")]
        public string? DiaDiemThi { get; set; }

        [Display(Name = "Phòng thi")]
        public string? PhongThi { get; set; }
        public int? MaGiangVien { get; set; }

        [Display(Name = "Lý do phúc khảo")]

        [Required(ErrorMessage = "Vui lòng nhập lý do phúc khảo")]
        public string? LyDo { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn môn học")]
        [Display(Name = "Môn học")]
        public int? MaMonHoc { get; set; }
        public SelectList? DanhSachMonHoc { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn học kỳ")]
        [Display(Name = "Học kỳ")]
        public int? MaHocKy { get; set; }
        public SelectList? DanhSachHocKy { get; set; }

        public DateTime NgayBatDauPhucKhao { get; set; }
        public DateTime NgayKetThucPhucKhao { get; set; }
    }
}
