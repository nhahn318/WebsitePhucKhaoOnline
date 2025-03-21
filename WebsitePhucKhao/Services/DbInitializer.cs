using Microsoft.AspNetCore.Identity;
using WebsitePhucKhao.Models;

public static class DbInitializer
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Danh sách các Role cần khởi tạo
        var roles = new List<string> { "Admin", "GiangVien", "SinhVien", "NhanVienPhongDaoTao" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Kiểm tra nếu Admin đã tồn tại
        var adminEmail = "admin@domain.com";
        var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

        if (existingAdmin == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                HoTen = "Administrator",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var password = "Admin@123"; // Đặt mật khẩu mặc định
            var result = await userManager.CreateAsync(adminUser, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
