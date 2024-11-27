namespace FacultyWebsite.Models
{
    public class NewCourses
    {

        public string StudentSsn { get; set; }
        public List<StudentCourse> CurrentStudentCourses { get; set; }

        public StudentCourse NewCourse { get; set; }

        public string ConfirmStudentSsn { get; set; }
        public string ConfirmCoursenum { get; set; }

    }
}
