using System.ComponentModel.DataAnnotations;
namespace WebsitePhucKhao.ViewModels {
    public class UploadBaiThiViewModel {
        public int MaDon { get; set; }

        [Required(ErrorMessage = "Chọn giảng viên chấm lại")]
        public long MaGiangVien { get; set; }
    }

}
