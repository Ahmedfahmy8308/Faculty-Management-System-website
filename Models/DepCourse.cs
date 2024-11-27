using System;
using System.Collections.Generic;

namespace FacultyWebsite.Models;

public partial class DepCourse
{
    public string Depnum { get; set; } = null!;

    public string Coursenum { get; set; } = null!;

    public virtual Course CoursenumNavigation { get; set; } = null!;
}
