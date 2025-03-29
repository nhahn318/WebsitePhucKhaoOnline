using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebsitePhucKhao.Models {
    public class BangDiem {
        [Key]
        public int Id { get; set; }

        public long MaSinhVien { get; set; }
        [ForeignKey(nameof(MaSinhVien))]
        public SinhVien? SinhVien { get; set; }

        public int MaMonHoc { get; set; }
        [ForeignKey(nameof(MaMonHoc))]
        public MonHoc? MonHoc { get; set; }

        public double? DiemGiuaKy { get; set; }
        public double? DiemCuoiKy { get; set; }
        public double? DiemTongKet { get; set; }

        public bool TrangThaiPhucKhao { get; set; }

        public int MaHocKy { get; set; }
        [ForeignKey(nameof(MaHocKy))]
        public HocKy? HocKy { get; set; }

        public int MaNamHoc { get; set; }
        [ForeignKey(nameof(MaNamHoc))]
        public NamHoc? NamHoc { get; set; }

        public DateTime? NgayThi { get; set; }

        public int LanThi { get; set; } = 1;
    }

}
