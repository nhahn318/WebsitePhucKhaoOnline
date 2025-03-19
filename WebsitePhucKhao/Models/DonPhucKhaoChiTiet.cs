using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsitePhucKhao.Models {
   
    namespace WebsitePhucKhao.Models {
        public class DonPhucKhaoChiTiet {
            [Key]
            public int MaChiTiet { get; set; }  // Khóa chính

            [ForeignKey("DonPhucKhao")]
            public int MaDon { get; set; }  // Khóa ngoại liên kết với đơn phúc khảo
            public DonPhucKhao? DonPhucKhao { get; set; }

            [ForeignKey("GiangVien")]
            public int? MaGiangVienPhucKhao { get; set; } // Giảng viên được phân công chấm
            public GiangVien? GiangVienPhucKhao { get; set; }

            public float? DiemSauPhucKhao { get; set; } // Điểm sau phúc khảo
            public string? NhanXet { get; set; }  // Nhận xét từ giảng viên

            public DateTime NgayChamLai { get; set; } = DateTime.Now;  // Ngày giảng viên chấm lại

            public string TrangThaiPhucKhao { get; set; } = "Chưa chấm"; // Trạng thái phúc khảo (Chưa chấm, Đã chấm)

            [ForeignKey("NhanVienPhongDaoTao")]
            public int? MaNhanVienDuyet { get; set; }  // Nhân viên duyệt đơn này
            public NhanVienPhongDaoTao? NhanVienDuyet { get; set; }
        }

    }

}
