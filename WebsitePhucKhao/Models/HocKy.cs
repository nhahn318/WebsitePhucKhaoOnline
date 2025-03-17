using System.ComponentModel.DataAnnotations;

namespace WebsitePhucKhao.Models {
    public class HocKy {
        [Key]
        public int MaHocKy { get; set; }

        [Required]
        [StringLength(50)]
        public string TenHocKy { get; set; } // Ví dụ: "Học kỳ 1 - 2025"

        public ICollection<MonHoc>? MonHocs { get; set; }
        public ICollection<LichThi>? LichThis { get; set; }
    }

}
