using Microsoft.AspNetCore.Identity;


namespace FacultyWebsite.Data
{
    public class AppUser : IdentityUser
    {
        public string SSN { get; set; }


    }
}
