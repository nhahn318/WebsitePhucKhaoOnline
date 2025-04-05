using WebsitePhucKhao.Models;
using WebsitePhucKhao.ViewModels;

namespace WebsitePhucKhao.Repositories {
    public interface IPhucKhaoRepository {
        Task<SinhVien?> GetSinhVienByEmailAsync(string email);
        Task<List<DonPhucKhao>> GetDanhSachPhucKhaoSinhVienAsync(long maSinhVien);
        Task<DonPhucKhao?> GetDonPhucKhaoAsync(int id);
        Task<SuaPhucKhaoViewModel?> GetSuaPhucKhaoViewModelAsync(int id);
        Task<bool> CapNhatDonPhucKhaoAsync(SuaPhucKhaoViewModel model);
        Task<bool> XacNhanXoaDonAsync(int id);
        Task<bool> XoaDonAsync(int id);
        Task<YeuCauPhucKhaoViewModel?> GetYeuCauPhucKhaoViewModelAsync(string email);
        Task<bool> TaoDonPhucKhaoAsync(YeuCauPhucKhaoViewModel model);
        Task<List<DonPhucKhao>> GetDanhSachChoDuyetAsync();
        Task<bool> DuyetDonAsync(int id, long? maNV);
        Task<UploadBaiThiViewModel?> GetUploadViewModelAsync(int maDon);
        Task<bool> LuuUploadAsync(UploadBaiThiViewModel model, List<IFormFile> files, long? maNV);
        Task<ChiTietUploadViewModel?> GetChiTietUploadAsync(int maDon);
        Task<EditUploadViewModel?> GetEditUploadAsync(int maDon);
        Task<bool> LuuChinhSuaUploadAsync(EditUploadViewModel model);
        Task<List<MonHoc>> GetMonHocListAsync();
        Task<List<HocKy>> GetHocKyListAsync();
        Task<List<NamHoc>> GetNamHocListAsync();
        Task<List<GiangVien>> GetGiangVienListAsync();

        Task<List<DonPhucKhaoSauKhiChamViewModel>> GetDanhSachPhucKhaoViewModelAsync(long maSinhVien);
        Task<double?> GetDiemHienTaiAsync(long maSinhVien, int maMonHoc, int maHocKy, int maNamHoc);



        void Update(DonPhucKhao don);
        Task SaveAsync();


    }
}
