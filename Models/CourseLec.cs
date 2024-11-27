using System;
using System.Collections.Generic;

namespace FacultyWebsite.Models;

public partial class CourseLec
{
    public string CourseNum { get; set; } = null!;

    public string LecturePath { get; set; } = null!;

    public string LectureNum { get; set; } = null!;

    public virtual Course CourseNumNavigation { get; set; } = null!;
}
