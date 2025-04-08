using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsitePhucKhao.Models {
    public class Lop {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaLop { get; set; }

        [Required]
        [StringLength(50)]
        public string? TenLop { get; set; }

        [ForeignKey("Khoa")]
        public int? MaKhoa { get; set; }
        public Khoa? Khoa { get; set; }

        public ICollection<SinhVien>? SinhViens { get; set; }
    }
}
