﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebsitePhucKhao.Models {
    public class ApplicationUser : IdentityUser 
    {

        [Required]
        public string HoTen { get; set; } = string.Empty;

        [ForeignKey("SinhVien")]
        public int? MaSinhVien { get; set; }

        public SinhVien? SinhVien { get; set; }

        public int? MaGiangVien { get; set; }
        public GiangVien? GiangVien { get; set; }

        public int? MaNhanVienPhongDaoTao { get; set; }
        public NhanVienPhongDaoTao? NhanVienPhongDaoTao { get; set; }
    }
}
