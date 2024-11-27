namespace FacultyWebsite.Models
{
    public class AddDoctor
    {
        public string Ssn { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Phone { get; set; }
        



        [AllowedExtensions(new string[] { ".jpg", ".jpeg" }, ErrorMessage = "Only JPG files are allowed.")]
        public IFormFile Image { get; set; }
    }


}
