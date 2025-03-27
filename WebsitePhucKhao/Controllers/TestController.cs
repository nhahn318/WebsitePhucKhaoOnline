using Microsoft.AspNetCore.Mvc;
using WebsitePhucKhao.Repositories;

namespace WebsitePhucKhao.Controllers
{
    public class TestController : Controller {
        private readonly ICustomEmailSender _emailSender;

        public TestController(ICustomEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public async Task<IActionResult> SendTestEmail()
        {
            var toEmail = "nhtony318@gmail.com"; // 👈 Thay bằng email bạn muốn nhận thử
            var subject = "📧 Test gửi email từ hệ thống";
            var message = "Xin chào! Đây là email test được gửi từ ASP.NET Core";

            try
            {
                await _emailSender.SendEmailAsync(toEmail, subject, message);
                return Content("✅ Email test đã được gửi đến " + toEmail);
            }
            catch (Exception ex)
            {
                return Content("❌ Gửi email thất bại: " + ex.Message);
            }
        }
    }
}
