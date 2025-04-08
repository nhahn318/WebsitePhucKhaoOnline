using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebsitePhucKhao.Enums;

namespace WebsitePhucKhao.Models {

    public class DonPhucKhaoChiTiet {
        [Key]
        public int MaChiTiet { get; set; }  // Khóa chính

        public int MaDon { get; set; }  // Khóa ngoại liên kết với đơn phúc khảo
        
        [ForeignKey("MaDon")]
        public DonPhucKhao? DonPhucKhao { get; set; }

        [ForeignKey("GiangVien")]
        public long? MaGiangVien { get; set; } // Giảng viên được phân công chấm
        public GiangVien? GiangVien { get; set; }

        public float? DiemSauPhucKhao { get; set; } // Điểm sau phúc khảo
        public string? NhanXet { get; set; }  // Nhận xét từ giảng viên

        public DateTime NgayChamLai { get; set; } = DateTime.Now;  // Ngày giảng viên chấm lại

        public TrangThaiPhucKhao TrangThai { get; set; }

        [ForeignKey("NhanVienPhongDaoTao")]
        public long? MaNhanVienDuyet { get; set; }  // Nhân viên duyệt đơn này
        public NhanVienPhongDaoTao? NhanVienDuyet { get; set; }


        [ForeignKey("SinhVien")]
        public long? MaSinhVien { get; set; }
        public SinhVien? SinhVien { get; set; }

        [ForeignKey("MonHoc")]
        public int? MaMonHoc { get; set; }
        public MonHoc? MonHoc { get; set; }
        public string? BaiGiaiTayUrl { get; set; }

    }
}
