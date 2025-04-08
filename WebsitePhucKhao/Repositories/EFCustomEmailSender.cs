using System.Net.Mail;
using System.Net;

namespace WebsitePhucKhao.Repositories {
    public class EFCustomEmailSender  : ICustomEmailSender {
        private readonly IConfiguration _config;

        public EFCustomEmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var email = _config["EmailSettings:Email"];
            var password = _config["EmailSettings:Password"];

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(email, password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(email),
                Subject = subject,
                Body = message,
                IsBodyHtml = false
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
