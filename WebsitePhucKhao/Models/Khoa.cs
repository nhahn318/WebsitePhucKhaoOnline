using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsitePhucKhao.Models {
    public class Khoa {
        [Key]
        public int MaKhoa { get; set; }

        [Required]
        [StringLength(100)]
        public string? TenKhoa { get; set; }

        public ICollection<ChuyenNganh>? ChuyenNganhs { get; set; }
    }

}
