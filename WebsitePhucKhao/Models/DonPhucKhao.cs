using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsitePhucKhao.Models {
    public class DonPhucKhao {
        [Key]
        public int MaDon { get; set; }

        [Required]
        public int MaSinhVien { get; set; }

        [ForeignKey("MaSinhVien")]
        public SinhVien? SinhVien { get; set; }

        [Required]
        public string? MonHoc { get; set; }

        public float DiemHienTai { get; set; }
        public float DiemMongMuon { get; set; }

        public int HocKy { get; set; }
        public string? NamHoc { get; set; }

        public DateTime NgayThi { get; set; }
        public string? CaThi { get; set; }
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

    
    }

}
