using Microsoft.AspNetCore.Mvc;
using WebsitePhucKhao.Models;
using WebsitePhucKhao.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsitePhucKhao.ViewModels;
using WebsitePhucKhao.Enums;
using Microsoft.AspNetCore.Authorization;

namespace WebsitePhucKhao.Areas.NhanVien.Controllers
{
    [Area("NhanVien")]
    [Authorize(Roles = "NhanVien")]
    public class NhanVienPhongDaoTaoController : Controller
    {
        private readonly INhanVienPhongDaoTaoRepository _nhanVienRepository;
        private readonly ApplicationDbContext _context;

        public NhanVienPhongDaoTaoController(INhanVienPhongDaoTaoRepository nhanVienRepository, ApplicationDbContext context)
        {
            _nhanVienRepository = nhanVienRepository;
            _context = context;
        }

        public async Task<IActionResult> ChiTietSauPhucKhao(int maDon)
        {
            var don = await _context.DonPhucKhaos
                .Include(d => d.ChiTietPhucKhao)
                .Include(d => d.HinhAnhBaiThis)
                .FirstOrDefaultAsync(d => d.MaDon == maDon);

            if (don == null) return NotFound();

            var chiTiet = don.ChiTietPhucKhao;

            var vm = new DonPhucKhaoSauKhiChamViewModel
            {
                MaDon = don.MaDon,
                NgayGui = don.NgayGui,
                TrangThai = don.TrangThai.ToFriendlyString(),
                DiemHienTai = don.DiemHienTai,
                DiemSauPhucKhao = chiTiet?.DiemSauPhucKhao,
                NhanXet = chiTiet?.NhanXet,
                BaiGiaiTayUrl = chiTiet?.BaiGiaiTayUrl,
                DanhSachAnh = don.HinhAnhBaiThis?.ToList() ?? new List<HinhAnhBaiThi>()
            };

            return View(vm);
        }
    }
}
