# Website Phúc Khảo Online

## Giới thiệu
Website Phúc Khảo Online là một ứng dụng web được xây dựng bằng ASP.NET Core MVC, phục vụ cho việc quản lý và xử lý các yêu cầu phúc khảo điểm thi của sinh viên. Hệ thống cho phép sinh viên nộp đơn phúc khảo trực tuyến và các bên liên quan (giảng viên, nhân viên phòng đào tạo) có thể xử lý các yêu cầu này một cách hiệu quả.

## Công nghệ sử dụng
- ASP.NET Core MVC 9.0
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- Bootstrap (cho giao diện người dùng)

## Các tính năng chính
1. **Quản lý người dùng**
   - Phân quyền người dùng (Sinh viên, Giảng viên, Nhân viên, Admin)
   - Đăng nhập/Đăng xuất
   - Quản lý thông tin cá nhân

2. **Quản lý phúc khảo**
   - Sinh viên có thể xem điểm thi và nộp đơn phúc khảo
   - Giảng viên có thể xem và xử lý đơn phúc khảo
   - Nhân viên phòng đào tạo quản lý quy trình phúc khảo
   - Admin quản lý toàn bộ hệ thống

3. **Quản lý dữ liệu**
   - Quản lý thông tin sinh viên
   - Quản lý thông tin giảng viên
   - Quản lý môn học
   - Quản lý lớp học
   - Quản lý bảng điểm
   - Quản lý học kỳ và năm học

## Cài đặt và chạy dự án

### Yêu cầu hệ thống
- .NET SDK 9.0
- SQL Server
- Visual Studio 2022 hoặc VS Code

### Các bước cài đặt
1. Clone repository về máy local
2. Mở solution trong Visual Studio
3. Cập nhật connection string trong file `appsettings.json`
4. Chạy các migration để tạo database:
   ```
   dotnet ef database update
   ```
5. Chạy dự án:
   ```
   dotnet run
   ```

### Tài khoản mặc định
- Email: admin@phuckhao.edu.vn
- Password: Admin@123

## Cấu trúc dự án
- `Areas/`: Chứa các module riêng biệt cho từng vai trò (Admin, Giảng viên, Nhân viên, Sinh viên)
- `Controllers/`: Chứa các controller xử lý request
- `Models/`: Chứa các model dữ liệu
- `Views/`: Chứa các view hiển thị
- `ViewModels/`: Chứa các view model
- `Repositories/`: Chứa các repository pattern để tương tác với database
- `wwwroot/`: Chứa các file tĩnh (CSS, JavaScript, Images)
- `Migrations/`: Chứa các file migration để quản lý cấu trúc database
- `Enums/`: Chứa các enum được sử dụng trong dự án
- `Properties/`: Chứa các cấu hình của dự án
- `bin/`: Chứa các file đã được biên dịch
- `obj/`: Chứa các file trung gian trong quá trình biên dịch
- `appsettings.json`: File cấu hình chính của ứng dụng
- `appsettings.Development.json`: File cấu hình cho môi trường development
- `Program.cs`: File khởi tạo và cấu hình ứng dụng
- `WebsitePhucKhao.csproj`: File cấu hình project
- `WebsitePhucKhao.sln`: File solution của Visual Studio

## Đóng góp
Mọi đóng góp đều được chào đón. Vui lòng tạo issue hoặc pull request để đóng góp vào dự án.

## Giấy phép
Dự án này được phân phối dưới giấy phép MIT. Xem file `LICENSE` để biết thêm chi tiết. 
