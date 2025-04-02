Go
INSERT INTO Khoas (TenKhoa)
VALUES (N'Kinh tế');
Go
INSERT INTO Lops (TenLop, MaKhoa)
VALUES 
(N'KT101', 1),
(N'KT102', 1),
(N'KT103', 1);
Go
INSERT INTO ChuyenNganhs (TenChuyenNganh, MaKhoa)
VALUES 
(N'Kinh tế quốc tế', 1),
(N'Tài chính ngân hàng', 1),
(N'Quản trị kinh doanh', 1);
Go
INSERT INTO NamHocs (TenNamHoc)
VALUES (N'2023-2024'), (N'2024-2025'), (N'2025-2026');
Go
INSERT INTO GiangViens (HoTen, MaGiangVien, Email, SoDienThoai, MaKhoa)
VALUES (N'Giảng viên A', 123, N'GVA@gmail.com', 123, 1);
Go
INSERT INTO HocKys (TenHocKy, MaNamHoc, NgayBatDauPhucKhao, NgayKetThucPhucKhao)
VALUES (N'2A', 3, '2025-03-24', '2025-04-03');
Go
INSERT INTO MonHocs (TenMonHoc, MaHocKy, MaGiangVien)
VALUES (N'Lập trình C#', 1, 123);
go
INSERT INTO MonHocs (TenMonHoc, MaHocKy, MaGiangVien)
VALUES (N'Lập trình C++', 1, 123);
Go
INSERT INTO NhanVienPhongDaoTaos (MaNhanVienPhongDaoTao, HoTen, Email, SoDienThoai, ChucVu)
VALUES (123, 'NVA', 'NVA@gmail.com', '12', 'a');
Go

INSERT INTO LichThis (NgayThi, CaThi, PhongThi, DiaDiemThi, MaMonHoc, MaHocKy, MaNamHoc)
VALUES ('2025-03-15', 'Ca 2', 'P.203', 'Cơ sở A', 1, 1, 3);
go
INSERT INTO LichThis (NgayThi, CaThi, PhongThi, DiaDiemThi, MaMonHoc, MaHocKy, MaNamHoc)
VALUES ('2025-03-15', 'Ca 1', 'P.202', 'Cơ sở B', 2, 1, 3);
go
INSERT INTO SinhViens (MaSinhVien, HoTen, Email, SoDienThoai, MaKhoa, MaChuyenNganh, MaLop, MatKhau)
VALUES (123, 'SVA',  'SVA@gmail.com', '12', 1, 1, 1, '123');
go
INSERT INTO SinhViens (MaSinhVien, HoTen, Email, SoDienThoai, MaKhoa, MaChuyenNganh, MaLop, MatKhau)
VALUES (234, 'SVB',  'SVB@gmail.com', '12', 1, 1, 1, '234');
go
INSERT INTO BangDiems 
(MaSinhVien, MaMonHoc, MaHocKy, MaNamHoc, NgayThi, LanThi, DiemGiuaKy, DiemCuoiKy, DiemTongKet, TrangThaiPhucKhao)
VALUES
(123, 1, 1, 3, '2025-03-15', 1, 6.0, 7.0, 6.5, 0);
Go
INSERT INTO BangDiems 
(MaSinhVien, MaMonHoc, MaHocKy, MaNamHoc, NgayThi, LanThi, DiemGiuaKy, DiemCuoiKy, DiemTongKet, TrangThaiPhucKhao)
VALUES
(123, 1, 1, 3, '2025-03-15', 1, 6.0, 7.0, 6.5, 0);
Go
INSERT INTO BangDiems 
(MaSinhVien, MaMonHoc, MaHocKy, MaNamHoc, NgayThi, LanThi, DiemGiuaKy, DiemCuoiKy, DiemTongKet, TrangThaiPhucKhao)
VALUES
(123, 2, 1, 3, '2025-03-15', 1, 6.0, 7.0, 6.5, 0);
go
