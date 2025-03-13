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



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();


app.UseAuthorization();

app.MapRazorPages();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
