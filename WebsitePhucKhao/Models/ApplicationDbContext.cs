using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebsitePhucKhao.Models {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<SinhVien> SinhViens { get; set; }
        public DbSet<DonPhucKhao> DonPhucKhaos { get; set; }
        public DbSet<HinhAnhBaiThi> HinhAnhBaiThis { get; set; }
        public DbSet<KetQuaPhucKhao> KetQuaPhucKhaos { get; set; }

        public DbSet<ChuyenNganh> ChuyenNganhs { get; set; }
        public DbSet<Khoa> Khoas { get; set; }



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


            modelBuilder.Entity<DonPhucKhao>()
                .HasOne(p => p.SinhVien)
                .WithMany(s => s.DonPhucKhaos)
                .HasForeignKey(p => p.MaSinhVien);

            modelBuilder.Entity<HinhAnhBaiThi>()
                .HasOne(h => h.DonPhucKhao)
                .WithMany(p => p.HinhAnhBaiThis)
                .HasForeignKey(h => h.MaHinhAnh);

            modelBuilder.Entity<KetQuaPhucKhao>()
                .HasOne(k => k.DonPhucKhao)
                .WithOne(p => p.KetQuaPhucKhao)
                .HasForeignKey<KetQuaPhucKhao>(k => k.MaDon);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.SinhVien)
                .WithOne(s => s.User)
                .HasForeignKey<ApplicationUser>(a => a.MaSinhVien);
        }
    }
}
