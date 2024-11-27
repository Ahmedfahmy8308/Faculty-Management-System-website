using System;
using System.Collections.Generic;

namespace FacultyWebsite.Models;

public partial class LectureAttendance
{
    public string CourseNum { get; set; } = null!;

    public string LectureNum { get; set; } = null!;

    public double Preiod { get; set; }

    public string? Description { get; set; }

    public string DocSsn { get; set; } = null!;

    public string LectureNumCourseNum { get; set; } = null!;
}
