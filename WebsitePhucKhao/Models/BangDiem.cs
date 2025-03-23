using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsitePhucKhao.Models {
    public class BangDiem {
        [Key]
        public int Id { get; set; } // Khóa chính tự tăng

        [ForeignKey("SinhVien")]
        public long MaSinhVien { get; set; }
        public SinhVien? SinhVien { get; set; }

        [ForeignKey("MonHoc")]
        public int MaMonHoc { get; set; }
        public MonHoc? MonHoc { get; set; }

        public double? DiemGiuaKy { get; set; }
        public double? DiemCuoiKy { get; set; }
        public double? DiemTongKet { get; set; }

        public bool TrangThaiPhucKhao { get; set; } // Có phúc khảo hay không
    }
}
