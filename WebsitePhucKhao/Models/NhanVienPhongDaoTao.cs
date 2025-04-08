using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebsitePhucKhao.Models {
    public class NhanVienPhongDaoTao {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long MaNhanVienPhongDaoTao { get; set; }

        [Required]
        [StringLength(255)]
        public string? HoTen { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(15)]
        public string? SoDienThoai { get; set; }

        public string? ChucVu { get; set; } // VD: Trưởng phòng, Nhân viên

        public ICollection<DonPhucKhao>? DonPhucKhaos { get; set; }
        public ICollection<DonPhucKhaoChiTiet>? DonPhucKhaoChiTiets { get; set; }

    }
}
