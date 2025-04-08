using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsitePhucKhao.Models {
    public class HinhAnhBaiThi {
        [Key]
        public int MaHinhAnh { get; set; }

        [Required]
        public int MaDon { get; set; }

        [ForeignKey("MaDon")]
        public DonPhucKhao? DonPhucKhao { get; set; }

        [Required]
        public string? DuongDanFile { get; set; }

        public DateTime NgayTaiLen { get; set; } = DateTime.Now;
    }
}
