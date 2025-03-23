using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsitePhucKhao.Models {
    public class MonHoc {
        [Key]
        public int MaMonHoc { get; set; }

        [Required]
        [StringLength(100)]
        public string? TenMonHoc { get; set; }

        [ForeignKey("HocKy")]
        public int? MaHocKy { get; set; }
        public HocKy? HocKy { get; set; } // Liên kết với Học Kỳ

        public ICollection<LichThi>? LichThis { get; set; }

        public long? MaGiangVien { get; set; }  
        public GiangVien? GiangVien { get; set; }

        // Quan hệ giữa MonHoc và DonPhucKhao (1:N)
        public ICollection<DonPhucKhao>? DonPhucKhaos { get; set; }  // Một môn học có thể có nhiều đơn phúc khảo

        public ICollection<BangDiem>? BangDiems { get; set; }
    }

}
