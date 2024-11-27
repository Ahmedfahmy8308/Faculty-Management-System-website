using System;
using System.Collections.Generic;

namespace FacultyWebsite.Models;

public partial class Course
{
    public string Coursenum { get; set; } = null!;

    public string Coursename { get; set; } = null!;

    public int? CreditHuors { get; set; }

    public string? BookPdf { get; set; }

    public string? CourseDescription { get; set; }

    public string CourseLevel { get; set; } = null!;

    public virtual ICollection<CourseAssignment> CourseAssignments { get; set; } = new List<CourseAssignment>();

    public virtual ICollection<CourseLec> CourseLecs { get; set; } = new List<CourseLec>();

    public virtual ICollection<DocCourse> DocCourses { get; set; } = new List<DocCourse>();

    public virtual ICollection<PreviousCourse> PreviousCourses { get; set; } = new List<PreviousCourse>();

    public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
}
