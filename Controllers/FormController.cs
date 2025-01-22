using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MimeKit;
using Saudi_FormEmail.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Saudi_FormEmail.Controllers
{
    public class FormController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public FormController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ViewSubmissions()
        {
            var submissions = await _context.ContactFormSubmissions.OrderByDescending(s => s.SubmittedAt).ToListAsync();
            return View(submissions);
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromForm] ContactFormModel model)
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

            return Ok("Email sent successfully!");
        }
    }
}
