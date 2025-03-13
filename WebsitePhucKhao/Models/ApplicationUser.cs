using Microsoft.AspNetCore.Identity;
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
    }
}
