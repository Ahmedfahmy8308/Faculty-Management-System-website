using System.ComponentModel.DataAnnotations;

namespace FacultyWebsite.Models
{
    public class LogInViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

     }
}
