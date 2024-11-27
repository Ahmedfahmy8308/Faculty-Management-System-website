using System;
using System.Collections.Generic;

namespace FacultyWebsite.Models;

public partial class Doctor
{
    public string Ssn { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Fname { get; set; } = null!;

    public string Lname { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Id { get; set; }
}
