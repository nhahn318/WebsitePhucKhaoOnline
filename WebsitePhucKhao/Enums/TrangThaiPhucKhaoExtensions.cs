namespace WebsitePhucKhao.Enums {
    public static class TrangThaiPhucKhaoExtensions {
        public static string ToFriendlyString(this TrangThaiPhucKhao trangThai)
        {
            return trangThai switch
            {
                TrangThaiPhucKhao.ChoXacNhan => "Chờ xác nhận",
                TrangThaiPhucKhao.DaDuyet => "Đã duyệt",
                TrangThaiPhucKhao.ChoCham => "Chờ chấm",
                TrangThaiPhucKhao.DaCham => "Đã chấm",
                TrangThaiPhucKhao.TuChoi => "Từ chối",
                _ => "Không rõ"
            };
        }
    }
}
