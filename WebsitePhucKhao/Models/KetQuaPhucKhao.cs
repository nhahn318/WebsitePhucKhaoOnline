using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsitePhucKhao.Models {
    public class KetQuaPhucKhao {
        [Key]
        public int MaDon { get; set; }

        [ForeignKey("MaDon")]
        public DonPhucKhao? DonPhucKhao { get; set; }

        public float DiemCuoiCung { get; set; }
        public string? GhiChu { get; set; }
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;
    }
}
