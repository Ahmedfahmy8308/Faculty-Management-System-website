using System;
using System.Collections.Generic;

namespace FacultyWebsite.Models;

public partial class Department
{
    public string Depnum { get; set; } = null!;

    public string Depname { get; set; } = null!;

    public string? Ssn { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
