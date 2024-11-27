using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace FacultyWebsite.Models
{
    public class AddStudent
    {

        public string Ssn { get; set; } 
        public string Email { get; set; }
        public string Password { get; set; } 
        public string Username { get; set; } 
        public string Seatnum { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }  
        public string IdentityId { get; set; } 
        public string Phone { get; set; } 
        public string Depnum { get; set; }
        
        public int Level { get; set; }
        public double Gpa { get; set; }
        
        
        [AllowedExtensions(new string[] { ".jpg", ".jpeg" }, ErrorMessage = "Only JPG files are allowed.")]
        public IFormFile Image { get; set; }
    }



    public class AllowedExtensionsAttribute : ValidationAttribute
        {
            private readonly string[] _extensions;

            public AllowedExtensionsAttribute(string[] extensions)
            {
                _extensions = extensions;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is IFormFile file)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if (!_extensions.Contains(extension.ToLower()))
                    {
                        return new ValidationResult(GetErrorMessage());
                    }
                }

                return ValidationResult.Success;
            }

            public string GetErrorMessage()
            {
                return $"Only files with extensions: {string.Join(", ", _extensions)} are allowed.";
            }
        }
}
