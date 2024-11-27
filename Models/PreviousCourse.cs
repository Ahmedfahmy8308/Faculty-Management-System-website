using System;
using System.Collections.Generic;

namespace FacultyWebsite.Models;

public partial class PreviousCourse
{
    public string StudentId { get; set; } = null!;

    public string CourseNum { get; set; } = null!;

    public string CourseName { get; set; } = null!;

    public string Degree { get; set; } = null!;

    public string Result { get; set; }

    public string SsnNum { get; set; } = null!;

    public virtual Course CourseNumNavigation { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
