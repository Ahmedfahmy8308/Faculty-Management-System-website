using Microsoft.AspNetCore.Mvc;
using FacultyWebsite.Models;
using FacultyWebsite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.IO;
using FacultyWebsite.Models;
using FacultyWebsite.Data;
using System.Data;


namespace FacultyWebsite.Controllers
{
    [Authorize(Roles = "student")]
    public class StudentController : Controller
    {

        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IHostingEnvironment hosting;

        DBAppContext smsContext = new DBAppContext();
        IdentityAspContext identityContext = new IdentityAspContext();

        public StudentController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHostingEnvironment hosting)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.hosting = hosting;
        }

        [HttpGet]
        public IActionResult Index()
        {
            
            var user = userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)).Result;
            var student = smsContext.Students.FirstOrDefault(s => s.Username == user.UserName);
            
            return View(student);
        }

        [HttpGet]
        public IActionResult StudentData()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Courses()
        {
            return View();
        }
    }
}
