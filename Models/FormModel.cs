namespace Saudi_FormEmail.Models
{
    public class FormModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Position { get; set; }
        public string Country { get; set; }
        public IFormFile File { get; set; }
    }
}
