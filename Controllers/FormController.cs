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
        [HttpPost("SaveForm")]
        public async Task<IActionResult> SaveForm([FromForm] FormModel model)
        {
            try
            {
                string uniqueFileName = null;
                if (model.File != null && model.File.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.File.CopyToAsync(stream);
                    }
                }

                var submission = new ContactFormSubmission
                {
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    Gender = model.Gender,
                    Message = model.Message,
                    Title = model.Title,
                    Company = model.Company,
                    Position = model.Position,
                    Country = model.Country,
                    Path = uniqueFileName == null ? "" : uniqueFileName,
                    SubmittedAt = DateTime.UtcNow
                };

                _context.ContactFormSubmissions.Add(submission);
                await _context.SaveChangesAsync();

                ViewData["Message"] = "تم الإرسال بنجاح";
            }
            catch {
                ViewData["Message"] = "حدث خطأ. حاول مرة أخري";
            }

            return View("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetFile/{fileName}")]
        public IActionResult GetFile(string fileName)
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound();
                }

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                var fileExtension = Path.GetExtension(fileName).ToLower();

                var contentType = fileExtension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".pdf" => "application/pdf",
                    ".doc" or ".docx" => "application/msword",
                    _ => "application/octet-stream"
                };

                return File(fileBytes, contentType, fileName);
            }
            catch
            {
                return null;
            }
        }
    }
}
