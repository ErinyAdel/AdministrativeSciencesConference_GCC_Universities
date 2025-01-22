using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Saudi_FormEmail.Models;

namespace Saudi_FormEmail.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public ContactController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail([FromForm] ContactFormModel model)
        {
            try
            {
                var submission = new ContactFormSubmission
                {
                    Name = model.Name,
                    Email = model.Email,
                    Message = model.Message,
                    SubmittedAt = DateTime.UtcNow
                };

                _context.ContactFormSubmissions.Add(submission);
                await _context.SaveChangesAsync();

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Your App", "faken1245@gmail.com"));
                emailMessage.To.Add(new MailboxAddress(model.Name, model.Email));
                emailMessage.Subject = "Contact Form Submission";
                emailMessage.Body = new TextPart("plain")
                {
                    Text = $"Name: {model.Name}\nEmail: {model.Email}\nMessage: {model.Message}"
                };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.sendgrid.net", 587, SecureSocketOptions.StartTls);
                string sendGridApiKey = _configuration["SendGridAPIKey"];
                await smtp.AuthenticateAsync("apikey", sendGridApiKey);
                await smtp.SendAsync(emailMessage);
                await smtp.DisconnectAsync(true);

                ViewData["Message"] = "تم الإرسال بنجاح";
            }
            catch
            {
                ViewData["Message"] = "حدث خطأ. حاول مرة أخري";
            }

            return View("ContactUs");
        }
    }
}
