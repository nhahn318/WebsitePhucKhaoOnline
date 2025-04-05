using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Độ dài tối thiểu
    options.Password.RequiredLength = 2;

    // Không yêu cầu chữ số (vì mật khẩu của bạn toàn số)
    options.Password.RequireDigit = false;

    // Không yêu cầu ký tự đặc biệt
    options.Password.RequireNonAlphanumeric = false;

    // Không yêu cầu ký tự in hoa
    options.Password.RequireUppercase = false;

    // Không yêu cầu ký tự in thường
    options.Password.RequireLowercase = false;


});


builder.Services.AddRazorPages();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ISinhVienRepository, EFSinhVienRepository>();
builder.Services.AddScoped<IChuyenNganhRepository, EFChuyenNganhRepository>();
builder.Services.AddScoped<IKhoaRepository, EFKhoaRepository>();
builder.Services.AddScoped<IGiangVienRepository, EFGiangVienRepository>();
builder.Services.AddScoped<INhanVienPhongDaoTaoRepository, EFNhanVienPhongDaoTaoRepository>();
builder.Services.AddScoped<IHocKyRepository, EFHocKyRepository>();
builder.Services.AddScoped<INamHocRepository, EFNamHocRepository>();
builder.Services.AddScoped<IMonHocRepository, EFMonHocRepository>();
builder.Services.AddScoped<IBangDiemRepository, EFBangDiemRepository>();
builder.Services.AddScoped<ILopRepository, EFLopRepository>();
builder.Services.AddScoped<IDonPhucKhaoChiTietRepository, EFDonPhucKhaoChiTietRepository>();
builder.Services.AddScoped<IPhucKhaoRepository, EFPhucKhaoRepository>();
builder.Services.AddScoped<ICustomEmailSender, EFCustomEmailSender>();





var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "SinhVien", "GiangVien", "NhanVien", "Admin" };

    foreach (var role in roles)
    {
        var exists = await roleManager.RoleExistsAsync(role);
        if (!exists)
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}


using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string adminEmail = "admin@phuckhao.edu.vn";
    string adminPassword = "Admin@123";

    var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
    if (existingAdmin == null)
    {
        var adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            HoTen = "Quản trị viên"
        };

        var result = await userManager.CreateAsync(adminUser, adminPassword);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}





// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Fix lỗi font tiếng Việt khi trả JSON
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/PhucKhao/GetLichThiInfo"))
    {
        context.Response.Headers["Content-Type"] = "application/json; charset=utf-8";
    }
    await next.Invoke();
});
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();



app.Run();