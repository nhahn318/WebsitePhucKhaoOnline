using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebsitePhucKhao.Enums;


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
        public int? MaNamHoc { get; set; }

        [ForeignKey("MaNamHoc")]
        public NamHoc? NamHoc { get; set; }


        public string? NhomLop { get; set; }
        public string? DiaDiemThi { get; set; }
        public string? PhongThi { get; set; }
        public long? MaGiangVien { get; set; }
        public GiangVien? GiangVien { get; set; }
        public long? MaNhanVienPhongDaoTao { get; set; }
        public NhanVienPhongDaoTao? NhanVienPhongDaoTao { get; set; }
        public TrangThaiPhucKhao TrangThai { get; set; }
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

        public DonPhucKhaoChiTiet? ChiTietPhucKhao { get; set; }


        //gửi email thông báo sinh viên
        public string EmailSinhVien { get; set; }

        public bool DaGuiEmail { get; set; }
    }

}
