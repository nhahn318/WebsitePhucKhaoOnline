using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebsitePhucKhao.Models {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<SinhVien> SinhViens { get; set; }
        public DbSet<DonPhucKhao> DonPhucKhaos { get; set; }
        public DbSet<HinhAnhBaiThi> HinhAnhBaiThis { get; set; }
        public DbSet<KetQuaPhucKhao> KetQuaPhucKhaos { get; set; }
        public DbSet<NhanVienPhongDaoTao> NhanVienPhongDaoTaos { get; set; }
        public DbSet<GiangVien> GiangViens { get; set; }


        public DbSet<ChuyenNganh> ChuyenNganhs { get; set; }
        public DbSet<Khoa> Khoas { get; set; }

        public DbSet<Lop> Lops { get; set; }

        public DbSet<HocKy> HocKys { get; set; }
        public DbSet<MonHoc> MonHocs { get; set; }
        public DbSet<LichThi> LichThis { get; set; }
        public DbSet<NamHoc> NamHocs { get; set; }
        public DbSet<BangDiem> BangDiems { get; set; }
        public DbSet<DonPhucKhaoChiTiet> DonPhucKhaoChiTiets { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mối quan hệ giữa Khoa và Chuyên Ngành
            modelBuilder.Entity<Khoa>()
                .HasMany(k => k.ChuyenNganhs)
                .WithOne(cn => cn.Khoa)
                .HasForeignKey(cn => cn.MaKhoa)
                .OnDelete(DeleteBehavior.Cascade);

            // Mối quan hệ giữa SinhVien và Khoa
            modelBuilder.Entity<SinhVien>()
                .HasOne(sv => sv.Khoa)
                .WithMany()
                .HasForeignKey(sv => sv.MaKhoa)
                .OnDelete(DeleteBehavior.Restrict); // Tránh xóa Khoa ảnh hưởng đến SinhVien

            // Mối quan hệ giữa SinhVien và ChuyenNganh
            modelBuilder.Entity<SinhVien>()
                .HasOne(sv => sv.ChuyenNganh)
                .WithMany(cn => cn.SinhViens)
                .HasForeignKey(sv => sv.MaChuyenNganh)
                .OnDelete(DeleteBehavior.Restrict); // Tránh xóa Chuyên ngành ảnh hưởng đến SinhVien

            // Mối quan hệ giữa SinhVien và DonPhucKhao
            modelBuilder.Entity<DonPhucKhao>()
                .HasOne(p => p.SinhVien)
                .WithMany(s => s.DonPhucKhaos)
                .HasForeignKey(p => p.MaSinhVien)
                .OnDelete(DeleteBehavior.Cascade);

            // Mối quan hệ giữa DonPhucKhao và HinhAnhBaiThi
            modelBuilder.Entity<HinhAnhBaiThi>()
                .HasOne(h => h.DonPhucKhao)
                .WithMany(p => p.HinhAnhBaiThis)
                .HasForeignKey(h => h.MaDon)
                .OnDelete(DeleteBehavior.Cascade);

            // Mối quan hệ giữa DonPhucKhao và KetQuaPhucKhao
            modelBuilder.Entity<KetQuaPhucKhao>()
                .HasOne(k => k.DonPhucKhao)
                .WithOne(p => p.KetQuaPhucKhao)
                .HasForeignKey<KetQuaPhucKhao>(k => k.MaDon)
                .OnDelete(DeleteBehavior.Cascade);

            // Mối quan hệ giữa ApplicationUser và SinhVien (1:1)
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.SinhVien)
                .WithOne(s => s.User)
                .HasForeignKey<ApplicationUser>(a => a.MaSinhVien);

            // ======= Bổ sung quan hệ cho GiangVien và NhanVienPhongDaoTao =======

            // Mối quan hệ giữa DonPhucKhao và GiangVien (1:N)
            modelBuilder.Entity<DonPhucKhao>()
                .HasOne(d => d.GiangVien)
                .WithMany(g => g.DonPhucKhaos)
                .HasForeignKey(d => d.MaGiangVien)
                .OnDelete(DeleteBehavior.SetNull); // Nếu giảng viên bị xóa, đơn vẫn tồn tại

            // Mối quan hệ giữa KetQuaPhucKhao và GiangVien (1:N)
            modelBuilder.Entity<KetQuaPhucKhao>()
                .HasOne(k => k.GiangVien)
                .WithMany(g => g.KetQuaPhucKhaos)
                .HasForeignKey(k => k.MaGiangVien)
                .OnDelete(DeleteBehavior.Cascade);

            // Mối quan hệ giữa DonPhucKhao và NhanVienPhongDaoTao (1:N)
            modelBuilder.Entity<DonPhucKhao>()
                .HasOne(d => d.NhanVienPhongDaoTao)
                .WithMany(nv => nv.DonPhucKhaos)
                .HasForeignKey(d => d.MaNhanVienPhongDaoTao)
                .OnDelete(DeleteBehavior.SetNull); // Nếu nhân viên bị xóa, đơn vẫn tồn tại

            modelBuilder.Entity<SinhVien>()
                .HasOne(sv => sv.Lop)
                .WithMany(l => l.SinhViens)
                .HasForeignKey(sv => sv.MaLop)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Khoa>()
                 .HasMany(k => k.Lops)
                .WithOne(l => l.Khoa)
                .HasForeignKey(l => l.MaKhoa)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<HocKy>()
                .HasMany(hk => hk.MonHocs)
                .WithOne(mh => mh.HocKy)
                .HasForeignKey(mh => mh.MaHocKy)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MonHoc>()
                .HasMany(mh => mh.LichThis)
                .WithOne(lt => lt.MonHoc)
                .HasForeignKey(lt => lt.MaMonHoc)
                .OnDelete(DeleteBehavior.Cascade);

            // Mối quan hệ giữa DonPhucKhao và MonHoc
            modelBuilder.Entity<DonPhucKhao>()
                .HasOne(dp => dp.MonHoc)
                .WithMany(mh => mh.DonPhucKhaos)
                .HasForeignKey(dp => dp.MaMonHoc)
                .OnDelete(DeleteBehavior.Restrict); // Không xóa môn học nếu có đơn phúc khảo

            // (Tuỳ chọn) Mối quan hệ giữa DonPhucKhao và LichThi
            modelBuilder.Entity<DonPhucKhao>()
                .HasOne(dp => dp.LichThi)
                .WithMany(lt => lt.DonPhucKhaos)
                .HasForeignKey(dp => dp.MaLichThi)
                .OnDelete(DeleteBehavior.Restrict); // Không xóa lịch thi nếu có đơn phúc khảo

            // Mối quan hệ giữa MonHoc và GiangVien (1:M)
            modelBuilder.Entity<MonHoc>()
                .HasOne(mh => mh.GiangVien)
                .WithMany(gv => gv.MonHocs)
                .HasForeignKey(mh => mh.MaGiangVien)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<HocKy>()
                .HasOne(h => h.NamHoc)
                .WithMany(n => n.HocKys)
                .HasForeignKey(h => h.MaNamHoc);

            modelBuilder.Entity<BangDiem>()
                .HasOne(b => b.SinhVien)
                .WithMany(s => s.BangDiems)
                .HasForeignKey(b => b.MaSinhVien);

            modelBuilder.Entity<BangDiem>()
                .HasOne(b => b.MonHoc)
                .WithMany(m => m.BangDiems)
                .HasForeignKey(b => b.MaMonHoc);

            modelBuilder.Entity<DonPhucKhao>()
                .HasOne(d => d.ChiTietPhucKhao)
                .WithOne()
                .HasForeignKey<DonPhucKhaoChiTiet>(ct => ct.MaDon)
                .OnDelete(DeleteBehavior.Cascade);


            // Mối quan hệ giữa DonPhucKhaoChiTiet và GiangVien (1:N)
            modelBuilder.Entity<DonPhucKhaoChiTiet>()
                .HasOne(dpct => dpct.GiangVien)
                .WithMany(gv => gv.DonPhucKhaoChiTiets)
                .HasForeignKey(dpct => dpct.MaGiangVien)
                .OnDelete(DeleteBehavior.SetNull); // Nếu giảng viên bị xóa, đơn vẫn tồn tại

            // Mối quan hệ giữa DonPhucKhaoChiTiet và NhanVienPhongDaoTao (1:N)
            modelBuilder.Entity<DonPhucKhaoChiTiet>()
                .HasOne(dpct => dpct.NhanVienDuyet)
                .WithMany(nv => nv.DonPhucKhaoChiTiets)
                .HasForeignKey(dpct => dpct.MaNhanVienDuyet)
                .OnDelete(DeleteBehavior.SetNull); // Nếu nhân viên bị xóa, đơn vẫn tồn tại
        }

    }
}
