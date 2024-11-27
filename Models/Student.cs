using System;
using System.Collections.Generic;

namespace FacultyWebsite.Models;

public partial class Student
{
    public string Ssn { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string? Seatnum { get; set; }

    public string Fname { get; set; } = null!;

    public string Lname { get; set; } = null!;

    public string IdentityId { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Depnum { get; set; }

    public int? Level { get; set; }

    public double? Gpa { get; set; }

    public string? Id { get; set; }

    public string? ImgPath { get; set; }

    public virtual Department? DepnumNavigation { get; set; }

    public virtual ICollection<PreviousCourse> PreviousCourses { get; set; } = new List<PreviousCourse>();

    public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
}
