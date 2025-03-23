using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebsitePhucKhao.Models.WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Models {
    public class DonPhucKhao {
        [Key]
        public int MaDon { get; set; }

        [Required]
        public long MaSinhVien { get; set; }

        [ForeignKey("MaSinhVien")]
        public SinhVien? SinhVien { get; set; }

        public float DiemHienTai { get; set; }
        public float DiemMongMuon { get; set; }

        public int HocKy { get; set; }
        public string? NamHoc { get; set; }

        public string? NhomLop { get; set; }
        public string? DiaDiemThi { get; set; }
        public string? PhongThi { get; set; }
        public int? MaGiangVien { get; set; }
        public GiangVien? GiangVien { get; set; }
        public int? MaNhanVienPhongDaoTao { get; set; }
        public NhanVienPhongDaoTao? NhanVienPhongDaoTao { get; set; }
        public string TrangThai { get; set; } = "Đang chờ xử lý";
        public DateTime NgayGui { get; set; } = DateTime.Now;

        [Required]
        public string? LyDo { get; set; }

        public ICollection<HinhAnhBaiThi>? HinhAnhBaiThis { get; set; }
        public KetQuaPhucKhao? KetQuaPhucKhao { get; set; }

        [ForeignKey("MaLichThi")]
        public int? MaLichThi { get; set; } // Foreign key for LichThi
        public LichThi? LichThi { get; set; } // Navigation property

        [ForeignKey("MonHoc")]
        public int? MaMonHoc { get; set; }  // Khóa ngoại liên kết với MonHoc
        public MonHoc? MonHoc { get; set; }  // Thuộc tính điều hướng đến MonHoc

        public ICollection<DonPhucKhaoChiTiet>? DanhSachPhucKhaoChiTiet { get; set; }

    }

}
