using Microsoft.AspNetCore.Mvc;
using FacultyWebsite.Models;
using FacultyWebsite.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace FacultyWebsite.Controllers
{

    public class LoginController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        DBAppContext smsContext = new DBAppContext();




        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LogInViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                AppUser usermodel = await userManager.FindByEmailAsync(loginVM.EMail);

                if (usermodel != null)
                {
                    bool found = await userManager.CheckPasswordAsync(usermodel, loginVM.Password);
                    if (found)
                    {
                        await signInManager.SignInAsync(usermodel, false);

                        var roles = await userManager.GetRolesAsync(usermodel);

                        Console.WriteLine(string.Join(", ", roles)); 

                        if (roles.Contains("Affaire", StringComparer.OrdinalIgnoreCase))
                            return RedirectToAction("index", "Affaire");
                        else if (roles.Contains("Doctor", StringComparer.OrdinalIgnoreCase))
                            return RedirectToAction("index", "Doctor");
                        else if (roles.Contains("Student", StringComparer.OrdinalIgnoreCase))
                            return RedirectToAction("index", "Student");
                        else
                            ModelState.AddModelError("", "User does not have a valid role assigned.");

                        //if (User.IsInRole("Student"))
                        //    return RedirectToAction("");
                        //string id = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                        //Student st = smsContext.Students.FirstOrDefault(x => x.Id == id);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Wrong Email or Password");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User not found");
                }
            }

            return View(loginVM);
        }



        [Authorize]
        public IActionResult Logout()
        {
            signInManager.SignOutAsync();
            return RedirectToAction("index", "Home");
        }

        public IActionResult GetId()
        {
            Claim IdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return Content(IdClaim.Value);
        }

    }
}
