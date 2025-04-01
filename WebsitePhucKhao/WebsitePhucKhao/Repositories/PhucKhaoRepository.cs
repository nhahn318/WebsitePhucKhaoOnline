using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.Models;

namespace WebsitePhucKhao.Repositories
{
    public class PhucKhaoRepository
    {
        private readonly ApplicationDbContext _context;

        public PhucKhaoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DonPhucKhao>> GetNewNotificationsAsync()
        {
            // Lấy các đơn phúc khảo mới trong vòng 24h qua
            var oneDayAgo = DateTime.Now.AddDays(-1);
            return await _context.DonPhucKhaos
                .Include(d => d.SinhVien)
                .Where(d => d.NgayGui >= oneDayAgo && d.TrangThai == 0) // 0 = Chờ xác nhận
                .OrderByDescending(d => d.NgayGui)
                .Take(10)
                .ToListAsync();
        }

        public async Task<List<DonPhucKhao>> GetDanhSachChoDuyetAsync()
        {
            return await _context.DonPhucKhaos
                .Include(d => d.SinhVien)
                .Include(d => d.MonHoc)
                .Include(d => d.GiangVien)
                .Include(d => d.NhanVienPhongDaoTao)
                .Where(d => d.TrangThai == 0) // 0 = Chờ xác nhận
                .OrderByDescending(d => d.NgayGui)
                .ToListAsync();
        }

        public async Task<List<DonPhucKhao>> GetDonPhucKhaoAsync()
        {
            return await _context.DonPhucKhaos
                .Include(d => d.SinhVien)
                .Include(d => d.MonHoc)
                .Include(d => d.GiangVien)
                .Include(d => d.NhanVienPhongDaoTao)
                .OrderByDescending(d => d.NgayGui)
                .ToListAsync();
        }

        public async Task<List<GiangVien>> GetGiangVienListAsync()
        {
            return await _context.GiangViens.ToListAsync();
        }

        public async Task<List<HocKy>> GetHocKyListAsync()
        {
            return await _context.HocKys.ToListAsync();
        }

        public async Task<List<MonHoc>> GetMonHocListAsync()
        {
            return await _context.MonHocs.ToListAsync();
        }

        public async Task<List<NamHoc>> GetNamHocListAsync()
        {
            return await _context.NamHocs.ToListAsync();
        }

        public async Task<bool> Update(DonPhucKhao donPhucKhao)
        {
            try
            {
                _context.DonPhucKhaos.Update(donPhucKhao);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> XoaDonAsync(int id)
        {
            try
            {
                var don = await _context.DonPhucKhaos.FindAsync(id);
                if (don != null)
                {
                    _context.DonPhucKhaos.Remove(don);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
} 