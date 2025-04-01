using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WebsitePhucKhao.Enums;
using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                // Dữ liệu thống kê
                ViewBag.TotalStudents = _context.SinhViens?.Count() ?? 0;
                ViewBag.TotalTeachers = _context.GiangViens?.Count() ?? 0;
                ViewBag.TotalSubjects = _context.MonHocs?.Count() ?? 0;
                ViewBag.TotalAppeals = _context.DonPhucKhaos?.Count() ?? 0;

                // Thống kê đơn phúc khảo theo trạng thái
                ViewBag.PendingAppeals = _context.DonPhucKhaos?.Count(d => d.TrangThai == TrangThaiPhucKhao.ChoXacNhan) ?? 0;
                ViewBag.ApprovedAppeals = _context.DonPhucKhaos?.Count(d => d.TrangThai == TrangThaiPhucKhao.DaDuyet) ?? 0;
                ViewBag.WaitingForGrading = _context.DonPhucKhaos?.Count(d => d.TrangThai == TrangThaiPhucKhao.ChoCham) ?? 0;
                ViewBag.GradedAppeals = _context.DonPhucKhaos?.Count(d => d.TrangThai == TrangThaiPhucKhao.DaCham) ?? 0;
                ViewBag.RejectedAppeals = _context.DonPhucKhaos?.Count(d => d.TrangThai == TrangThaiPhucKhao.TuChoi) ?? 0;

                // Thống kê đơn phúc khảo theo học kỳ hiện tại
                var currentSemester = _context.HocKys?
                    .OrderByDescending(h => h.MaHocKy)
                    .FirstOrDefault();

                if (currentSemester != null)
                {
                    ViewBag.CurrentSemester = currentSemester.TenHocKy;
                    ViewBag.CurrentSemesterAppeals = _context.DonPhucKhaos?
                        .Count(d => d.MaHocKy == currentSemester.MaHocKy) ?? 0;
                }

                // Hoạt động gần đây
                if (_context.DonPhucKhaos != null)
                {
                    var recentActivities = _context.DonPhucKhaos
                        .Include(d => d.SinhVien)
                        .Include(d => d.MonHoc)
                        .OrderByDescending(d => d.NgayGui)
                        .Take(8)
                        .Select(d => new
                        {
                            Time = d.NgayGui.ToString("dd/MM/yyyy HH:mm"),
                            Student = d.SinhVien != null ? d.SinhVien.HoTen : "N/A",
                            StudentId = d.MaSinhVien,
                            Subject = d.MonHoc != null ? d.MonHoc.TenMonHoc : "N/A",
                            CurrentGrade = d.DiemHienTai,
                            ExpectedGrade = d.DiemMongMuon,
                            Status = d.TrangThai.ToString(),
                            StatusString = DashboardHelper.GetTrangThaiString(d.TrangThai),
                            StatusColor = DashboardHelper.GetStatusColor(d.TrangThai)
                        })
                        .ToList();

                    ViewBag.RecentActivities = recentActivities;
                }

                // Thống kê theo môn học
                if (_context.DonPhucKhaos != null && _context.MonHocs != null)
                {
                    var subjectStats = _context.DonPhucKhaos
                        .GroupBy(d => d.MaMonHoc)
                        .Select(g => new
                        {
                            MonHocId = g.Key,
                            Count = g.Count()
                        })
                        .Join(_context.MonHocs,
                            s => s.MonHocId,
                            m => m.MaMonHoc,
                            (s, m) => new
                            {
                                Subject = m.TenMonHoc,
                                Count = s.Count
                            })
                        .OrderByDescending(s => s.Count)
                        .Take(5)
                        .ToList();

                    ViewBag.SubjectStats = subjectStats;
                }

                // Thống kê theo giảng viên
                if (_context.DonPhucKhaos != null && _context.GiangViens != null)
                {
                    var teacherStats = _context.DonPhucKhaoChiTiets
                        .GroupBy(d => d.MaGiangVien)
                        .Select(g => new
                        {
                            GiangVienId = g.Key,
                            Count = g.Count()
                        })
                        .Join(_context.GiangViens,
                            s => s.GiangVienId,
                            m => m.MaGiangVien,
                            (s, m) => new
                            {
                                TeacherName = m.HoTen,
                                Count = s.Count
                            })
                        .OrderByDescending(s => s.Count)
                        .Take(5)
                        .ToList();

                    ViewBag.TeacherStats = teacherStats;
                }

                // Thống kê điểm phúc khảo
                if (_context.DonPhucKhaos != null && _context.DonPhucKhaoChiTiets != null)
                {
                    var totalGraded = _context.DonPhucKhaoChiTiets
                        .Count(d => d.DiemSauPhucKhao.HasValue);

                    var gradesChanged = _context.DonPhucKhaoChiTiets
                        .Count(d => d.DiemSauPhucKhao.HasValue && 
                                   d.DonPhucKhao != null && 
                                   d.DiemSauPhucKhao.Value != d.DonPhucKhao.DiemHienTai);

                    ViewBag.TotalGraded = totalGraded;
                    ViewBag.GradesChanged = gradesChanged;
                    ViewBag.GradesChangedPercent = totalGraded > 0 
                        ? (gradesChanged * 100 / totalGraded) 
                        : 0;
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ khi truy cập database
                ViewBag.Error = "Không thể tải dữ liệu: " + ex.Message;
            }

            return View();
        }
    }

    public static class DashboardHelper
    {
        public static string GetTrangThaiString(TrangThaiPhucKhao trangThai)
        {
            return trangThai switch
            {
                TrangThaiPhucKhao.ChoXacNhan => "Chờ xác nhận",
                TrangThaiPhucKhao.DaDuyet => "Đã duyệt",
                TrangThaiPhucKhao.ChoCham => "Chờ chấm",
                TrangThaiPhucKhao.DaCham => "Đã chấm",
                TrangThaiPhucKhao.TuChoi => "Từ chối",
                _ => "Không xác định"
            };
        }

        public static string GetStatusColor(TrangThaiPhucKhao status)
        {
            return status switch
            {
                TrangThaiPhucKhao.ChoXacNhan => "warning",
                TrangThaiPhucKhao.DaDuyet => "primary",
                TrangThaiPhucKhao.ChoCham => "info",
                TrangThaiPhucKhao.DaCham => "success",
                TrangThaiPhucKhao.TuChoi => "danger",
                _ => "secondary"
            };
        }
    }
}
