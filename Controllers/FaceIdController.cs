using FacultyWebsite.Data;
using FacultyWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System;
using System.IO;
using System.Linq;

namespace FacultyWebsite.Controllers
{
    public class FaceIdController : Controller
    {
        DBAppContext smsContext = new DBAppContext();
        LogInViewModel logInViewModel = new LogInViewModel();
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public FaceIdController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("api/folders")]
        public IActionResult GetFolders()
        {
            string[] directories = Directory.GetDirectories("wwwroot/Face ID/labels");
            string[] folderNames = directories.Select(Path.GetFileName).ToArray();

            return Json(folderNames);
        }
        [HttpPost]
        [Route("api/GetUserName")]

        async public Task<IActionResult> Post([FromBody] FaceDetectionRequest request)
        {
            Console.WriteLine(request.Couresenum);
                Console.WriteLine(request.Lecnum);
            if (request != null)
            {
                var username = request.Result.Split(" ")[0];
                var pres = request.Result.Split(" (")[1];
                var confidence = pres.Split(")")[0];
                float conf = float.Parse(confidence);
                Console.WriteLine(request.Couresenum);
                Console.WriteLine(request.Lecnum);
                
                if (username == "Unknown")
                {
                    return Ok("Unknown user");
                }
                else if (conf < 0.35)
                {
                    return Ok("False");
                }
                else
                {
                    AppUser usermodel = await userManager.FindByNameAsync(username);
                    Console.WriteLine(usermodel);
                    if (usermodel != null)
                    {
                        await signInManager.SignInAsync(usermodel, false);

                        var roles = await userManager.GetRolesAsync(usermodel);

                        if (roles.Contains("Affairs", StringComparer.OrdinalIgnoreCase))
                            return Ok("Affairs");
                        else if (roles.Contains("doctor", StringComparer.OrdinalIgnoreCase))
                            return Ok("doctor");
                        else if (roles.Contains("Student", StringComparer.OrdinalIgnoreCase))
                            return Ok("Student");
                    }
                    else
                    {
                        return Ok("False");
                    }
                }
            }
            else
            {
                return BadRequest("Invalid request body");
            }

            // Add a default return statement outside of the if-else blocks
            return BadRequest("Invalid request body");
        }

        public class FaceDetectionRequest
        {
            public string Result { get; set; }

            public string Couresenum { get; set; }
            public string Lecnum { get; set; }
        }

    }
}
