using System;
using System.Collections.Generic;

namespace FacultyWebsite.Models;

public partial class StudentCourse
{
    public string Ssn { get; set; } = null!;

    public string Coursenum { get; set; } = null!;

    public string SsnNum { get; set; } = null!;

    public virtual Course CoursenumNavigation { get; set; } = null!;

    public virtual Student SsnNavigation { get; set; } = null!;
}
