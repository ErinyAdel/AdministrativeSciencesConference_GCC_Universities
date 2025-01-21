using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Saudi_FormEmail.Models;

namespace Saudi_FormEmail
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<ContactFormSubmission> ContactFormSubmissions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
