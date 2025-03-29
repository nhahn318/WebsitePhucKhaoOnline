using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsitePhucKhao.Models {
    public class LichThi {
        [Key]
        public int MaLichThi { get; set; }

        [Required]
        public DateTime NgayThi { get; set; }

        [ForeignKey("HocKy")]
        public int? MaHocKy { get; set; }
        public HocKy? HocKy { get; set; } // Liên kết với Học Kỳ

        [ForeignKey("MonHoc")]
        public int? MaMonHoc { get; set; }
        public MonHoc? MonHoc { get; set; } // Liên kết với Môn Học

        [ForeignKey("NamHoc")]
        public int? MaNamHoc { get; set; }
        public NamHoc? NamHoc { get; set; }

        public string? CaThi { get; set; }
        public string? DiaDiemThi { get; set; }
        public string? PhongThi { get; set; }

        public ICollection<DonPhucKhao>? DonPhucKhaos { get; set; }
    }

}
