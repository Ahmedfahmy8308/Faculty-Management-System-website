using System;
using System.Collections.Generic;

namespace FacultyWebsite.Models;

public partial class DocDepartment
{
    public string Depnum { get; set; } = null!;

    public string Ssn { get; set; } = null!;

    public virtual Department DepnumNavigation { get; set; } = null!;

    public virtual Doctor SsnNavigation { get; set; } = null!;
}
