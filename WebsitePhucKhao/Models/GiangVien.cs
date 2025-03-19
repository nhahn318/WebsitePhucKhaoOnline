using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebsitePhucKhao.Models.WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Models {
    public class GiangVien {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaGiangVien { get; set; }

        [Required]
        [StringLength(255)]
        public string? HoTen { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(15)]
        public string? SoDienThoai { get; set; }

        // Khóa ngoại liên kết với bảng Khoas
        [ForeignKey("Khoa")]
        public int MaKhoa { get; set; }
        public virtual Khoa? Khoa { get; set; }

        public ICollection<DonPhucKhao>? DonPhucKhaos { get; set; }
        public ICollection<KetQuaPhucKhao>? KetQuaPhucKhaos { get; set; }

        public ICollection<MonHoc>? MonHocs { get; set; }

        public ICollection<DonPhucKhaoChiTiet>? DonPhucKhaoChiTiets { get; set; }

    }
}
