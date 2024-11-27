namespace FacultyWebsite.Models
{
    public class SearchLectureAttendance
    {
        public string CourseNum { get; set; }
        public string LectureNum { get; set; }

        public string Decription { get; set; }
        public string period { get; set; }

        public List<AtendanceStudent> sa { get; set; }


    }

    public partial class AtendanceStudent
    {
        public string Fname { get; set; } = null!;

        public string Lname { get; set; } = null!;

        public string Level { get; set; } = null!;

        public string HasCourse { get; set; } = null!;
    }

}
