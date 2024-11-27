using System;
using System.Collections.Generic;

namespace FacultyWebsite.Models;

public partial class StudentAttendance
{
    public string CourseNum { get; set; } = null!;

    public string LectureNum { get; set; } = null!; 

    public string Stuusername { get; set; } = null!;

    public string StuusernameLectureNumCourseNum { get; set; } = null!;
}
