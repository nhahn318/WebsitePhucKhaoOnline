using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsitePhucKhao.Models {
    public class SinhVien {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MaSinhVien { get; set; }

        [Required]
        public string? HoTen { get; set; }

        [Required, EmailAddress]
        public string? Email { get; set; }

        public string? SoDienThoai { get; set; }
        // Liên kết với bảng Khoa
        [ForeignKey("Khoa")]
        public int MaKhoa { get; set; }
        public Khoa? Khoa { get; set; }

        // Liên kết với bảng ChuyenNganh
        [ForeignKey("ChuyenNganh")]
        public int MaChuyenNganh { get; set; }
        public ChuyenNganh? ChuyenNganh { get; set; }

        [ForeignKey("Lop")]
        public int? MaLop { get; set; }
        public Lop? Lop { get; set; }

        public string? MatKhau { get; set; }

        public ICollection<DonPhucKhao>? DonPhucKhaos { get; set; }

        public ApplicationUser? User { get; set; }

    
    }
}
