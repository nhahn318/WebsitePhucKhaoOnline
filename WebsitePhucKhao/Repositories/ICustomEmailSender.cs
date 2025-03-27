namespace WebsitePhucKhao.Repositories {
    public interface ICustomEmailSender {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
