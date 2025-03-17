using System.ComponentModel.DataAnnotations;

namespace WebsitePhucKhao.Models {
    public class NamHoc {
        [Key]
        public int MaNamHoc { get; set; }
        public string? TenNamHoc { get; set; }

        // Một năm học có nhiều học kỳ
        public ICollection<HocKy>? HocKys { get; set; }
    }
}
