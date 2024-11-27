namespace FacultyWebsite.Models
{
    public class SearchPreviousCourses
    {
        public string StudentSsn { get; set; }
        public List<PreviousCourse> previousCourses { get; set; }

        public PreviousCourse NewpreviousCourse { get; set; }

        public string ConfirmStudentSsn { get; set; }
        public string ConfirmCoursenum { get; set; }

    }
}
