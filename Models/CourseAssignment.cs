using System;
using System.Collections.Generic;

namespace FacultyWebsite.Models;

public partial class CourseAssignment
{
    public string CourseNum { get; set; } = null!;

    public string AssignmentNum { get; set; } = null!;

    public string AssignmentPath { get; set; } = null!;

    public virtual Course CourseNumNavigation { get; set; } = null!;
}
