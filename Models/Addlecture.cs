namespace FacultyWebsite.Models
{
    public class Addlecture
    {
        public string Coursenum { get; set; }

        public  string LecNum { get; set; } 


        [AllowedExtensions(new string[] { ".pdf" }, ErrorMessage = "Only pdf files are allowed.")]

        public IFormFile LecFile { get; set; }


    }
}
