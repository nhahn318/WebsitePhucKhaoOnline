using System.ComponentModel.DataAnnotations;

namespace WebsitePhucKhao.Models {
    public class HocKy {
        [Key]
        public int MaHocKy { get; set; }

        [Required]
        [StringLength(50)]
        public string? TenHocKy { get; set; } // Ví dụ: "Học kỳ 1 - 2025"

        public int? MaNamHoc { get; set; }
        public NamHoc? NamHoc { get; set; }

        public ICollection<MonHoc>? MonHocs { get; set; }
        public ICollection<LichThi>? LichThis { get; set; }


        [Required(ErrorMessage = "Vui lòng chọn ngày bắt đầu phúc khảo")]
        public DateTime NgayBatDauPhucKhao { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày kết thúc phúc khảo")]
        public DateTime NgayKetThucPhucKhao { get; set; }
    }

}
