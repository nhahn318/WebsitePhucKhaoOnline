using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Data {
    public class WebsitePhucKhaoContext : IdentityDbContext<ApplicationUser> {
        public WebsitePhucKhaoContext(DbContextOptions<WebsitePhucKhaoContext> options)
            : base(options)
        {
        }

        public DbSet<SinhVien> SinhViens { get; set; }
        public DbSet<GiangVien> GiangViens { get; set; }
        public DbSet<NhanVienPhongDaoTao> NhanVienPhongDaoTaos { get; set; }
        public DbSet<DonPhucKhao> DonPhucKhaos { get; set; }
        public DbSet<DonPhucKhaoChiTiet> DonPhucKhaoChiTiets { get; set; }
        public DbSet<HinhAnhBaiThi> HinhAnhBaiThis { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Bạn có thể thêm Fluent API config nếu cần
        }
    }
}
