using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebsitePhucKhao.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Khoas",
                columns: table => new
                {
                    MaKhoa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKhoa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Khoas", x => x.MaKhoa);
                });

            migrationBuilder.CreateTable(
                name: "NamHocs",
                columns: table => new
                {
                    MaNamHoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNamHoc = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NamHocs", x => x.MaNamHoc);
                });

            migrationBuilder.CreateTable(
                name: "NhanVienPhongDaoTaos",
                columns: table => new
                {
                    MaNhanVienPhongDaoTao = table.Column<int>(type: "int", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ChucVu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanVienPhongDaoTaos", x => x.MaNhanVienPhongDaoTao);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChuyenNganhs",
                columns: table => new
                {
                    MaChuyenNganh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenChuyenNganh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MaKhoa = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChuyenNganhs", x => x.MaChuyenNganh);
                    table.ForeignKey(
                        name: "FK_ChuyenNganhs_Khoas_MaKhoa",
                        column: x => x.MaKhoa,
                        principalTable: "Khoas",
                        principalColumn: "MaKhoa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GiangViens",
                columns: table => new
                {
                    MaGiangVien = table.Column<int>(type: "int", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    MaKhoa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiangViens", x => x.MaGiangVien);
                    table.ForeignKey(
                        name: "FK_GiangViens_Khoas_MaKhoa",
                        column: x => x.MaKhoa,
                        principalTable: "Khoas",
                        principalColumn: "MaKhoa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lops",
                columns: table => new
                {
                    MaLop = table.Column<int>(type: "int", nullable: false),
                    TenLop = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaKhoa = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lops", x => x.MaLop);
                    table.ForeignKey(
                        name: "FK_Lops_Khoas_MaKhoa",
                        column: x => x.MaKhoa,
                        principalTable: "Khoas",
                        principalColumn: "MaKhoa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HocKys",
                columns: table => new
                {
                    MaHocKy = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenHocKy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaNamHoc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HocKys", x => x.MaHocKy);
                    table.ForeignKey(
                        name: "FK_HocKys_NamHocs_MaNamHoc",
                        column: x => x.MaNamHoc,
                        principalTable: "NamHocs",
                        principalColumn: "MaNamHoc");
                });

            migrationBuilder.CreateTable(
                name: "SinhViens",
                columns: table => new
                {
                    MaSinhVien = table.Column<int>(type: "int", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaKhoa = table.Column<int>(type: "int", nullable: false),
                    MaChuyenNganh = table.Column<int>(type: "int", nullable: false),
                    MaLop = table.Column<int>(type: "int", nullable: true),
                    MatKhau = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinhViens", x => x.MaSinhVien);
                    table.ForeignKey(
                        name: "FK_SinhViens_ChuyenNganhs_MaChuyenNganh",
                        column: x => x.MaChuyenNganh,
                        principalTable: "ChuyenNganhs",
                        principalColumn: "MaChuyenNganh",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SinhViens_Khoas_MaKhoa",
                        column: x => x.MaKhoa,
                        principalTable: "Khoas",
                        principalColumn: "MaKhoa",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SinhViens_Lops_MaLop",
                        column: x => x.MaLop,
                        principalTable: "Lops",
                        principalColumn: "MaLop",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MonHocs",
                columns: table => new
                {
                    MaMonHoc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenMonHoc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MaHocKy = table.Column<int>(type: "int", nullable: true),
                    MaGiangVien = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonHocs", x => x.MaMonHoc);
                    table.ForeignKey(
                        name: "FK_MonHocs_GiangViens_MaGiangVien",
                        column: x => x.MaGiangVien,
                        principalTable: "GiangViens",
                        principalColumn: "MaGiangVien",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MonHocs_HocKys_MaHocKy",
                        column: x => x.MaHocKy,
                        principalTable: "HocKys",
                        principalColumn: "MaHocKy",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaSinhVien = table.Column<int>(type: "int", nullable: true),
                    MaGiangVien = table.Column<int>(type: "int", nullable: true),
                    GiangVienMaGiangVien = table.Column<int>(type: "int", nullable: true),
                    MaNhanVienPhongDaoTao = table.Column<int>(type: "int", nullable: true),
                    NhanVienPhongDaoTaoMaNhanVienPhongDaoTao = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_GiangViens_GiangVienMaGiangVien",
                        column: x => x.GiangVienMaGiangVien,
                        principalTable: "GiangViens",
                        principalColumn: "MaGiangVien");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_NhanVienPhongDaoTaos_NhanVienPhongDaoTaoMaNhanVienPhongDaoTao",
                        column: x => x.NhanVienPhongDaoTaoMaNhanVienPhongDaoTao,
                        principalTable: "NhanVienPhongDaoTaos",
                        principalColumn: "MaNhanVienPhongDaoTao");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_SinhViens_MaSinhVien",
                        column: x => x.MaSinhVien,
                        principalTable: "SinhViens",
                        principalColumn: "MaSinhVien");
                });

            migrationBuilder.CreateTable(
                name: "BangDiems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSinhVien = table.Column<int>(type: "int", nullable: false),
                    MaMonHoc = table.Column<int>(type: "int", nullable: false),
                    DiemGiuaKy = table.Column<double>(type: "float", nullable: true),
                    DiemCuoiKy = table.Column<double>(type: "float", nullable: true),
                    DiemTongKet = table.Column<double>(type: "float", nullable: true),
                    TrangThaiPhucKhao = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BangDiems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BangDiems_MonHocs_MaMonHoc",
                        column: x => x.MaMonHoc,
                        principalTable: "MonHocs",
                        principalColumn: "MaMonHoc",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BangDiems_SinhViens_MaSinhVien",
                        column: x => x.MaSinhVien,
                        principalTable: "SinhViens",
                        principalColumn: "MaSinhVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LichThis",
                columns: table => new
                {
                    MaLichThi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayThi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaHocKy = table.Column<int>(type: "int", nullable: true),
                    MaMonHoc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichThis", x => x.MaLichThi);
                    table.ForeignKey(
                        name: "FK_LichThis_HocKys_MaHocKy",
                        column: x => x.MaHocKy,
                        principalTable: "HocKys",
                        principalColumn: "MaHocKy");
                    table.ForeignKey(
                        name: "FK_LichThis_MonHocs_MaMonHoc",
                        column: x => x.MaMonHoc,
                        principalTable: "MonHocs",
                        principalColumn: "MaMonHoc",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonPhucKhaos",
                columns: table => new
                {
                    MaDon = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSinhVien = table.Column<int>(type: "int", nullable: false),
                    DiemHienTai = table.Column<float>(type: "real", nullable: false),
                    DiemMongMuon = table.Column<float>(type: "real", nullable: false),
                    HocKy = table.Column<int>(type: "int", nullable: false),
                    NamHoc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NhomLop = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaDiemThi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhongThi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaGiangVien = table.Column<int>(type: "int", nullable: true),
                    MaNhanVienPhongDaoTao = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayGui = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LyDo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaLichThi = table.Column<int>(type: "int", nullable: true),
                    MaMonHoc = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonPhucKhaos", x => x.MaDon);
                    table.ForeignKey(
                        name: "FK_DonPhucKhaos_GiangViens_MaGiangVien",
                        column: x => x.MaGiangVien,
                        principalTable: "GiangViens",
                        principalColumn: "MaGiangVien",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DonPhucKhaos_LichThis_MaLichThi",
                        column: x => x.MaLichThi,
                        principalTable: "LichThis",
                        principalColumn: "MaLichThi",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonPhucKhaos_MonHocs_MaMonHoc",
                        column: x => x.MaMonHoc,
                        principalTable: "MonHocs",
                        principalColumn: "MaMonHoc",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonPhucKhaos_NhanVienPhongDaoTaos_MaNhanVienPhongDaoTao",
                        column: x => x.MaNhanVienPhongDaoTao,
                        principalTable: "NhanVienPhongDaoTaos",
                        principalColumn: "MaNhanVienPhongDaoTao",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DonPhucKhaos_SinhViens_MaSinhVien",
                        column: x => x.MaSinhVien,
                        principalTable: "SinhViens",
                        principalColumn: "MaSinhVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HinhAnhBaiThis",
                columns: table => new
                {
                    MaHinhAnh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDon = table.Column<int>(type: "int", nullable: false),
                    DuongDanFile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayTaiLen = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhAnhBaiThis", x => x.MaHinhAnh);
                    table.ForeignKey(
                        name: "FK_HinhAnhBaiThis_DonPhucKhaos_MaDon",
                        column: x => x.MaDon,
                        principalTable: "DonPhucKhaos",
                        principalColumn: "MaDon",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KetQuaPhucKhaos",
                columns: table => new
                {
                    MaDon = table.Column<int>(type: "int", nullable: false),
                    DiemCuoiCung = table.Column<float>(type: "real", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaGiangVien = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KetQuaPhucKhaos", x => x.MaDon);
                    table.ForeignKey(
                        name: "FK_KetQuaPhucKhaos_DonPhucKhaos_MaDon",
                        column: x => x.MaDon,
                        principalTable: "DonPhucKhaos",
                        principalColumn: "MaDon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KetQuaPhucKhaos_GiangViens_MaGiangVien",
                        column: x => x.MaGiangVien,
                        principalTable: "GiangViens",
                        principalColumn: "MaGiangVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GiangVienMaGiangVien",
                table: "AspNetUsers",
                column: "GiangVienMaGiangVien");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MaSinhVien",
                table: "AspNetUsers",
                column: "MaSinhVien",
                unique: true,
                filter: "[MaSinhVien] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_NhanVienPhongDaoTaoMaNhanVienPhongDaoTao",
                table: "AspNetUsers",
                column: "NhanVienPhongDaoTaoMaNhanVienPhongDaoTao");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BangDiems_MaMonHoc",
                table: "BangDiems",
                column: "MaMonHoc");

            migrationBuilder.CreateIndex(
                name: "IX_BangDiems_MaSinhVien",
                table: "BangDiems",
                column: "MaSinhVien");

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenNganhs_MaKhoa",
                table: "ChuyenNganhs",
                column: "MaKhoa");

            migrationBuilder.CreateIndex(
                name: "IX_DonPhucKhaos_MaGiangVien",
                table: "DonPhucKhaos",
                column: "MaGiangVien");

            migrationBuilder.CreateIndex(
                name: "IX_DonPhucKhaos_MaLichThi",
                table: "DonPhucKhaos",
                column: "MaLichThi");

            migrationBuilder.CreateIndex(
                name: "IX_DonPhucKhaos_MaMonHoc",
                table: "DonPhucKhaos",
                column: "MaMonHoc");

            migrationBuilder.CreateIndex(
                name: "IX_DonPhucKhaos_MaNhanVienPhongDaoTao",
                table: "DonPhucKhaos",
                column: "MaNhanVienPhongDaoTao");

            migrationBuilder.CreateIndex(
                name: "IX_DonPhucKhaos_MaSinhVien",
                table: "DonPhucKhaos",
                column: "MaSinhVien");

            migrationBuilder.CreateIndex(
                name: "IX_GiangViens_MaKhoa",
                table: "GiangViens",
                column: "MaKhoa");

            migrationBuilder.CreateIndex(
                name: "IX_HinhAnhBaiThis_MaDon",
                table: "HinhAnhBaiThis",
                column: "MaDon");

            migrationBuilder.CreateIndex(
                name: "IX_HocKys_MaNamHoc",
                table: "HocKys",
                column: "MaNamHoc");

            migrationBuilder.CreateIndex(
                name: "IX_KetQuaPhucKhaos_MaGiangVien",
                table: "KetQuaPhucKhaos",
                column: "MaGiangVien");

            migrationBuilder.CreateIndex(
                name: "IX_LichThis_MaHocKy",
                table: "LichThis",
                column: "MaHocKy");

            migrationBuilder.CreateIndex(
                name: "IX_LichThis_MaMonHoc",
                table: "LichThis",
                column: "MaMonHoc");

            migrationBuilder.CreateIndex(
                name: "IX_Lops_MaKhoa",
                table: "Lops",
                column: "MaKhoa");

            migrationBuilder.CreateIndex(
                name: "IX_MonHocs_MaGiangVien",
                table: "MonHocs",
                column: "MaGiangVien");

            migrationBuilder.CreateIndex(
                name: "IX_MonHocs_MaHocKy",
                table: "MonHocs",
                column: "MaHocKy");

            migrationBuilder.CreateIndex(
                name: "IX_SinhViens_MaChuyenNganh",
                table: "SinhViens",
                column: "MaChuyenNganh");

            migrationBuilder.CreateIndex(
                name: "IX_SinhViens_MaKhoa",
                table: "SinhViens",
                column: "MaKhoa");

            migrationBuilder.CreateIndex(
                name: "IX_SinhViens_MaLop",
                table: "SinhViens",
                column: "MaLop");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BangDiems");

            migrationBuilder.DropTable(
                name: "HinhAnhBaiThis");

            migrationBuilder.DropTable(
                name: "KetQuaPhucKhaos");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "DonPhucKhaos");

            migrationBuilder.DropTable(
                name: "LichThis");

            migrationBuilder.DropTable(
                name: "NhanVienPhongDaoTaos");

            migrationBuilder.DropTable(
                name: "SinhViens");

            migrationBuilder.DropTable(
                name: "MonHocs");

            migrationBuilder.DropTable(
                name: "ChuyenNganhs");

            migrationBuilder.DropTable(
                name: "Lops");

            migrationBuilder.DropTable(
                name: "GiangViens");

            migrationBuilder.DropTable(
                name: "HocKys");

            migrationBuilder.DropTable(
                name: "Khoas");

            migrationBuilder.DropTable(
                name: "NamHocs");
        }
    }
}
