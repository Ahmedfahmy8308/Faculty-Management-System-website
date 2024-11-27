namespace FacultyWebsite.Models
{
    public class AddCourse
    {
        public string Coursenum { get; set; } 

        public string Coursename { get; set; } 

        public int CreditHuors { get; set; }

        public string level  { get; set; }

        public string CourseDescription { get; set; }

        

        [AllowedExtensions(new string[] { ".jpg", ".jpeg" }, ErrorMessage = "Only JPG files are allowed.")]
        public IFormFile CourseCover { get; set; }
    }
}
