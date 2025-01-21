namespace Saudi_FormEmail.Models
{
    public class ContactFormSubmission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
