namespace FacultyWebsite.Models
{
    public class SearchCourse
    {
        public List<Course> Courses { get; set; }

        public string confirmCoursename { get; set; }

        public string confirmCoursenum { get; set; }
        public string confirmCreditHours { get; set; }
        public string confirmLevel { get; set; }
    }
}

