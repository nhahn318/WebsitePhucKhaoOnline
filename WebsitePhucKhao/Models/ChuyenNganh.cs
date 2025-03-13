using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsitePhucKhao.Models {
    public class ChuyenNganh {
        [Key]
        public int MaChuyenNganh { get; set; }

        [Required]
        [StringLength(100)]
        public string? TenChuyenNganh { get; set; }

        [ForeignKey("Khoa")]
        public int? MaKhoa { get; set; }

        public Khoa? Khoa { get; set; }
        public ICollection<SinhVien>? SinhViens { get; set; }
    }

}
