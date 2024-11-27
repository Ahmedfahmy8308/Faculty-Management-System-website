namespace FacultyWebsite.Models
{
    public class DoctorCourses
    {

        public string DoctorSsn { get; set; }
        public List<DocCourse> DoctorCourse { get; set; }

        public DocCourse NewDoctorCourse { get; set; }

        public string ConfirmDoctorSsn { get; set; }
        public string ConfirmCourseNum{ get; set; }



    }
}
