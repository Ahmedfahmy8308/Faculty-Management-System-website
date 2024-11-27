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
using Microsoft.EntityFrameworkCore;
using NuGet.LibraryModel;

namespace FacultyWebsite.Controllers
{
    [Authorize(Policy = "Doctor")]
    public class DoctorController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IHostingEnvironment hosting;


        DBAppContext smsContext = new DBAppContext();
        IdentityAspContext identityContext = new IdentityAspContext();
        AttendanceContext attendanceContext = new AttendanceContext();
        public DoctorController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHostingEnvironment hosting)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.hosting = hosting;
        }
        [HttpGet]
        public IActionResult index()
        {
            var user = userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)).Result;
            var doctor = smsContext.Doctors.FirstOrDefault(s => s.Username == user.UserName);

            return View(doctor);
        }
        public IActionResult CourseManagement()
        {

            return View();
        }

        [HttpGet]
        public IActionResult Mycourses()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DoctorData()
        {
            return View();
        }



        [HttpGet]
        public IActionResult Addlecture()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Addlecture(Addlecture model)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("Addlecture");

                var user = userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)).Result;
                var doctor = smsContext.Doctors.FirstOrDefault(s => s.Username == user.UserName);
                var course = smsContext.Courses.FirstOrDefault(s => s.Coursenum == model.Coursenum);
                if (course != null)
                {
                    if (smsContext.DocCourses.FirstOrDefault(x => x.SsnNum == (doctor.Ssn + model.Coursenum)) == null)
                    {
                        ViewData["result"] = "You are not the doctor of this course";
                        return View();
                    }
                    else
                    {
                        var lectures = Path.Combine("wwwroot", "Books", course.Coursenum, "lectures");
                        var filestream = new FileStream(Path.Combine(lectures, model.LecNum + ".pdf"), FileMode.Create);
                        await model.LecFile.CopyToAsync(filestream);
                        filestream.Close();

                        ViewData["result"] = "Lecture added successfully";
                        return View();
                    }

                }
                else
                {
                    ViewData["result"] = "Course not found";
                    return View();
                }
            }
            ViewData["result"] = "Invalid data";
            return View();
        }


        [HttpGet]
        public IActionResult AddAssignment()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAssignment(AddAssignment model)
        {
            if (ModelState.IsValid)
            {

                var user = userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)).Result;
                var doctor = smsContext.Doctors.FirstOrDefault(s => s.Username == user.UserName);
                var course = smsContext.Courses.FirstOrDefault(s => s.Coursenum == model.Coursenum);
                if (course != null)
                {
                    if (smsContext.DocCourses.FirstOrDefault(x => x.SsnNum == (doctor.Ssn + model.Coursenum)) == null)
                    {
                        ViewData["result"] = "You are not the doctor of this course";
                        return View();
                    }
                    else
                    {
                        var assignments = Path.Combine("wwwroot", "Books", course.Coursenum, "Assignments");

                        var filestream = new FileStream(Path.Combine(assignments, model.AssignNum + ".pdf"), FileMode.Create);
                        await model.AssignFile.CopyToAsync(filestream);
                        filestream.Close();

                        ViewData["result"] = "Assignment added successfully";
                        return View();
                    }

                }
                else
                {
                    ViewData["result"] = "Course not found";
                    return View();
                }
            }
            ViewData["result"] = "Invalid data";
            return View();
        }

        [HttpGet]
        public IActionResult NewLectureAttendance()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewLectureAttendance(NewLectureAttendance model)
        {
            if (ModelState.IsValid)
            {
                var user = userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)).Result;
                var doctor = smsContext.Doctors.FirstOrDefault(s => s.Username == user.UserName);
                var course = smsContext.Courses.FirstOrDefault(s => s.Coursenum == model.CourseNum);
                if (course != null)
                {
                    if (smsContext.DocCourses.FirstOrDefault(x => x.SsnNum == (doctor.Ssn + model.CourseNum)) == null)
                    {
                        ViewData["result"] = "You are not the doctor of this course";
                        return View();
                    }
                    else if (attendanceContext.LectureAttendances.FirstOrDefault(x => x.LectureNumCourseNum == model.LectureNum + model.CourseNum) != null)
                    {
                        ViewData["result"] = "Lecture attendance already exists";
                        return View();
                    }
                    else
                    {
                        attendanceContext.LectureAttendances.Add(new LectureAttendance
                        {
                            CourseNum = model.CourseNum,
                            DocSsn = doctor.Ssn,
                            LectureNum = model.LectureNum,
                            Preiod = Double.Parse( model.Preiod),
                            Description = model.Description,
                            LectureNumCourseNum = model.LectureNum + model.CourseNum
                        });
                        var ftd = new Faceidtempdata
                        {
                            CoureseNum = model.CourseNum,
                            LecNum = model.LectureNum
                        };

                        attendanceContext.SaveChanges();
                        ViewData["result"] = "Lecture attendance added successfully";
                        return RedirectToAction("TakeAttendancewithFaceid", ftd);
                    }

                }
                else
                {
                    ViewData["result"] = "Course not found";
                    return View();
                }
            }
            ViewData["result"] = "Invalid data";
            return View();
        }

        [HttpGet]
        public IActionResult AttendanceManagement()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TakeAttendancewithFaceid(Faceidtempdata model)
        {
            return View(model);
        }

        [HttpPost]
        [Route("api/GetUserNameAtendance")]
        async public Task<IActionResult> Post([FromBody] FaceDetectionRequest request)
        {
            if (request != null)
            {
                var username = request.Result.Split(" ")[0];
                var pres = request.Result.Split(" (")[1];
                var confidence = pres.Split(")")[0];
                float conf = float.Parse(confidence);
                Console.WriteLine(request.CourseCode);
                Console.WriteLine(request.LecNum);
                var coursecode = request.CourseCode;
                var lecnum = request.LecNum;


                if (username == "Unknown")
                {
                    return Ok("Unknown user");
                }
                else if (conf < 0.4)
                {
                    return Ok("False");
                }
                else
                {
                    AppUser usermodel = await userManager.FindByNameAsync(username);
                    Console.WriteLine(usermodel);
                    if (usermodel != null)
                    {
                        var roles = await userManager.GetRolesAsync(usermodel);

                        if (roles.Contains("Affairs", StringComparer.OrdinalIgnoreCase))
                            return Ok("False");
                        else if (roles.Contains("Doctor", StringComparer.OrdinalIgnoreCase))
                            return Ok("False");
                        else if (roles.Contains("Student", StringComparer.OrdinalIgnoreCase))
                        {
                            var student = smsContext.Students.FirstOrDefault(s => s.Username == usermodel.UserName);
                            bool hasCourse = smsContext.StudentCourses.FirstOrDefault(x => x.SsnNum == student.Ssn + coursecode) != null;

                            if (attendanceContext.StudentAttendances.FirstOrDefault(x => x.StuusernameLectureNumCourseNum == student.Username + lecnum + coursecode) != null)
                            {
                                return Ok("False");
                            }
                            else
                            {
                                attendanceContext.StudentAttendances.Add(new StudentAttendance
                                {
                                    CourseNum = coursecode,
                                    LectureNum = lecnum,
                                    Stuusername = student.Username,
                                    StuusernameLectureNumCourseNum = student.Username + lecnum + coursecode
                                });
                                attendanceContext.SaveChanges();
                                return Ok(new
                                {
                                    FirstName = student.Fname,
                                    LastName = student.Lname,
                                    Level = student.Level,
                                    HasCourse = hasCourse
                                });
                            }

                        }
                        else
                        {
                            return Ok("False");
                        }
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



        [HttpPost]
        [Route("api/CancelAttendance")]
        async public Task<IActionResult> Post([FromBody] CancelLectureAttendance request)
        {
            if (request != null)
            {


                var coursecode = request.CourseCode;
                var lecnum = request.LecNum;
                attendanceContext.LectureAttendances.RemoveRange(attendanceContext.LectureAttendances.Where(x => x.LectureNum == lecnum && x.CourseNum == coursecode));
                attendanceContext.StudentAttendances.RemoveRange(attendanceContext.StudentAttendances.Where(x => x.LectureNum == lecnum && x.CourseNum == coursecode));
                attendanceContext.SaveChanges();
                return Ok("Attendance canceled successfully");
            }
            
           
            return BadRequest("Invalid request body");
        }


        [HttpGet]
        public IActionResult SearchLectureAttendance()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchLectureAttendance(SearchLectureAttendance model)
        {
            
                var user = userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)).Result;
                var doctor = smsContext.Doctors.FirstOrDefault(s => s.Username == user.UserName);
                var course = smsContext.Courses.FirstOrDefault(s => s.Coursenum == model.CourseNum);
                if (course != null)
                {
                    if (smsContext.DocCourses.FirstOrDefault(x => x.SsnNum == (doctor.Ssn + model.CourseNum)) == null)
                    {
                        ViewData["result"] = "You are not the doctor of this course";
                        return View();
                    }
                    else
                    {
                        var lecture = attendanceContext.LectureAttendances.FirstOrDefault(x => x.LectureNum == model.LectureNum && x.CourseNum == model.CourseNum);
                        if (lecture != null)
                        {
                        
                            var students = attendanceContext.StudentAttendances.Where(x => x.LectureNum ==  model.LectureNum && x.CourseNum ==  model.CourseNum).ToList();
                            Console.WriteLine(students.Count);
                            var sa = new List<AtendanceStudent>();

                            foreach (var student in students)
                            {
                                var stu = smsContext.Students.FirstOrDefault(x => x.Username == student.Stuusername);
                                sa.Add(new AtendanceStudent
                                {
                                    Fname = stu.Fname,
                                    Lname = stu.Lname,
                                    Level = smsContext.Students.FirstOrDefault(x => x.Ssn == stu.Ssn ).Level.ToString(),
                                    HasCourse = smsContext.StudentCourses.FirstOrDefault(x => x.SsnNum == stu.Ssn + model.CourseNum) != null ? "Yes" : "No"
                                }); 
                            }
                            model.sa = sa; 
                            model.period = lecture.Preiod.ToString();
                            model.Decription = lecture.Description;
                            return View(model);
                        }
                        else
                        {
                            ViewData["result"] = "Lecture attendance not found";
                            return View();
                        }
                    }

                }
                else
                {
                    ViewData["result"] = "Course not found";
                    return View();
                }
            
            
            ViewData["result"] = "Invalid data";
            return View();
        }
    }

    public class FaceDetectionRequest
            {
                public string Result { get; set; }
                public string CourseCode { get; set; }
                public string LecNum { get; set; }

            }
    public class CancelLectureAttendance
        {
            public string CourseCode { get; set; }
            public string LecNum { get; set; }
        }
}



    


