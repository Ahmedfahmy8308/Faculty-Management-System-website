namespace FacultyWebsite.Models
{
    public class AddAssignment
    {
        public string Coursenum { get; set; }

        public string AssignNum { get; set; }


        [AllowedExtensions(new string[] { ".pdf" }, ErrorMessage = "Only pdf files are allowed.")]

        public IFormFile AssignFile { get; set; }
    }
}
