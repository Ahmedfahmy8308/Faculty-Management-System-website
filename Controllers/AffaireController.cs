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
    [Authorize(Policy = "affaire")]
    public class AffaireController : Controller
    {


        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IHostingEnvironment hosting;

        DBAppContext smsContext = new DBAppContext();
        IdentityAspContext identityContext = new IdentityAspContext();

        public AffaireController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHostingEnvironment hosting)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.hosting = hosting;
        }



        [HttpGet]
        public IActionResult index()
        {

            return View();

        }


        [HttpGet]
        public IActionResult Doctor()
        {
            return View();

        }

        [HttpGet]
        public IActionResult Student()
        {
            return View();

        }

        [HttpGet]
        public IActionResult course()
        {
            return View();

        }



        //***********************************************************************************Student************************************************


        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddStudent(AddStudent Stu)
        {
            if (ModelState.IsValid)
            {
                var usermodel = new AppUser
                {
                    UserName = Stu.Username,
                    Email = Stu.Email,
                    PhoneNumber = Stu.Phone,
                    SSN = Stu.Ssn

                };

                var student = new Student
                {
                    Fname = Stu.Fname,
                    Lname = Stu.Lname,
                    Username = Stu.Username,
                    Email = Stu.Email,
                    Phone = Stu.Phone,
                    Password = Stu.Password,
                    Ssn = Stu.Ssn,
                    Seatnum = Stu.Seatnum,
                    Depnum = Stu.Depnum,
                    Level = Stu.Level,
                    Gpa = Stu.Gpa,
                    Id = usermodel.Id,
                    ImgPath = Path.Combine("wwwroot", "Face ID", "Labels", Stu.Username, 1 + ".jpg"),
                    IdentityId = Stu.IdentityId



                };

                if (smsContext.Students.FirstOrDefault(x => x.Ssn == Stu.Ssn) != null)
                {
                    ModelState.AddModelError("SSN", "SSN already exists");
                    return View(Stu);
                }
                else if (smsContext.Students.FirstOrDefault(x => x.Email == Stu.Email) != null)
                {
                    ModelState.AddModelError("Email", "Email already exists");
                    return View(Stu);
                }
                else if (smsContext.Students.FirstOrDefault(x => x.Username == Stu.Username) != null)
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(Stu);
                }
                else if (smsContext.Students.FirstOrDefault(x => x.Phone == Stu.Phone) != null)
                {
                    ModelState.AddModelError("Phone", "Phone already exists");
                    return View(Stu);
                }
                else if (smsContext.Students.FirstOrDefault(x => x.IdentityId == Stu.IdentityId) != null)
                {
                    ModelState.AddModelError("IdentityId", "Identity");
                    return View(Stu);
                }
                else
                {
                    IdentityResult result = await userManager.CreateAsync(usermodel, Stu.Password);

                    if (result.Succeeded)
                    {
                        try
                        {
                            student.Id = identityContext.AspNetUsers.FirstOrDefault(x => x.Ssn == Stu.Ssn).Id;
                            smsContext.Students.Add(student);
                            smsContext.SaveChanges();
                            await userManager.AddToRoleAsync(usermodel, "Student");

                            if (Stu.Image != null)
                            {
                                Directory.CreateDirectory(Path.Combine("wwwroot", "Face ID", "Labels", Stu.Username));
                                var filestream = new FileStream(student.ImgPath, FileMode.Create);
                                await Stu.Image.CopyToAsync(filestream);
                                filestream.Close();
                                ViewData["result"] = "Student added successfully";
                                return View(Stu);
                                //Directory.Delete(Path.Combine(hosting.WebRootPath, "Face ID", "Labels", Stu.Username), true);
                                //System.IO.File.Delete(student.ImgPath);
                            }
                        }

                        catch (Exception e)
                        {
                            ModelState.AddModelError("Size", e.Message);
                            userManager.DeleteAsync(usermodel);
                            //Directory.Delete(Path.Combine(hosting.WebRootPath, "Face ID", "Labels", Stu.Username),true);
                            //System.IO.File.Delete(student.ImgPath);
                        }
                    }
                    else
                    {
                        smsContext.Students.Remove(student);
                        foreach (var erroritem in result.Errors)
                        {
                            ModelState.AddModelError("PassWord", erroritem.Description);
                        }
                    }
                }
            }
            ViewData["result"] = "can not add Student";
            return View(Stu);

        }

        //***********************************************************************************

        [HttpGet]
        public IActionResult DeleteStudent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStudent(DeleteStudent stu)
        {
            if (stu.confirmUsername != null)
            {
                var user = await userManager.FindByNameAsync(stu.confirmUsername);
                if (user == null)
                {
                    ViewData["result"] = "Username not found";
                    return View();
                }
                await userManager.DeleteAsync(user);
                System.IO.Directory.Delete(Path.Combine("wwwroot", "Face ID", "Labels", stu.confirmUsername), true);
                ViewData["result"] = "Student Deleted successfully";
                return View();
            }
            else if (smsContext.Students.FirstOrDefault(x => x.Ssn == stu.Ssn) != null)
            {

                var student = smsContext.Students.FirstOrDefault(x => x.Ssn == stu.Ssn);
                var Rstu = new DeleteStudent
                {
                    Ssn = student.Ssn,
                    Email = student.Email,
                    Password = student.Password,
                    Username = student.Username,
                    Seatnum = student.Seatnum,
                    Fname = student.Fname,
                    Lname = student.Lname,
                    IdentityId = student.IdentityId,
                    Phone = student.Phone,
                    Depnum = student.Depnum,
                    Level = student.Level,
                    Gpa = student.Gpa,
                    Id = student.Id,
                };

                return View(Rstu);
            }

            ViewData["result"] = "User not found";

            return View();

        }

        //***********************************************************************************

        [HttpGet]
        public IActionResult UpdateStudent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStudent(UpdateStudent stu)
        {

            if (stu.confirmUsername != null)
            {
                var user = await userManager.FindByNameAsync(stu.confirmUsername);
                if (user == null)
                {
                    ViewData["result"] = "Username not found";
                    return View();
                }
                else
                {

                    var student = smsContext.Students.FirstOrDefault(x => x.Username == stu.Username);
                    student.Fname = stu.Fname;
                    student.Lname = stu.Lname;
                    student.Phone = stu.Phone;
                    student.Seatnum = stu.Seatnum;
                    student.Depnum = stu.Depnum;
                    student.Level = stu.Level;
                    student.Gpa = stu.Gpa;
                    smsContext.Students.Update(student);
                    smsContext.SaveChanges();
                    ViewData["result"] = "Student Updated successfully";
                    return View();
                }
            }
            else if (smsContext.Students.FirstOrDefault(x => x.Ssn == stu.Ssn) != null)
            {

                var student = smsContext.Students.FirstOrDefault(x => x.Ssn == stu.Ssn);
                var Rstu = new UpdateStudent
                {
                    Ssn = student.Ssn,
                    Email = student.Email,
                    Password = student.Password,
                    Username = student.Username,
                    Seatnum = student.Seatnum,
                    Fname = student.Fname,
                    Lname = student.Lname,
                    IdentityId = student.IdentityId,
                    Phone = student.Phone,
                    Depnum = student.Depnum,
                    Level = student.Level,
                    Gpa = student.Gpa,
                    Id = student.Id,
                };
                return View(Rstu);
            }
            ViewData["result"] = "User not found";

            return View();


        }

        //***********************************************************************************

        [HttpGet]
        public IActionResult SearchStudent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchStudent(SearchStudent stu)
        {
            var student = new Student();
            if (smsContext.Students.FirstOrDefault(x => x.Ssn == stu.confirmSsn) != null)
            {
                student = smsContext.Students.FirstOrDefault(x => x.Ssn == stu.confirmSsn);

            }
            else if (smsContext.Students.FirstOrDefault(x => x.Phone == stu.confirmPhone) != null)
            {
                student = smsContext.Students.FirstOrDefault(x => x.Phone == stu.confirmPhone);
            }
            else if (smsContext.Students.FirstOrDefault(x => x.Email == stu.confirmEmail) != null)
            {
                student = smsContext.Students.FirstOrDefault(x => x.Email == stu.confirmEmail);
            }
            else if (smsContext.Students.FirstOrDefault(x => x.IdentityId == stu.confirmId) != null)
            {
                student = smsContext.Students.FirstOrDefault(x => x.IdentityId == stu.confirmId);
            }
            else if (smsContext.Students.FirstOrDefault(x => x.Username == stu.confirmUsername) != null)
            {
                student = smsContext.Students.FirstOrDefault(x => x.Username == stu.confirmUsername);
            }
            else
            {
                ViewData["result"] = "User not found";
                return View();
            }

            var Rstu = new SearchStudent();

            Rstu.Ssn = student.Ssn;
            Rstu.Email = student.Email;
            Rstu.Password = student.Password;
            Rstu.Username = student.Username;
            Rstu.Seatnum = student.Seatnum;
            Rstu.Fname = student.Fname;
            Rstu.Lname = student.Lname;
            Rstu.IdentityId = student.IdentityId;
            Rstu.Phone = student.Phone;
            Rstu.Depnum = student.Depnum;
            Rstu.Level = student.Level;
            Rstu.Gpa = student.Gpa;
            Rstu.Id = student.Id;

            return View(Rstu);


        }


        [HttpGet]
        public IActionResult PreviousCourses()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PreviousCourses(SearchPreviousCourses SPC)
        {
            if (SPC.StudentSsn!= null)
            {
                if (smsContext.Students.FirstOrDefault(x => x.Ssn == SPC.StudentSsn) == null)
                {
                    ViewData["result"] = "Student not found";
                    return View();
                }
                var previousCoursesForThisStudent = smsContext.PreviousCourses.Where(x => x.StudentId == SPC.StudentSsn).ToList();
                if (previousCoursesForThisStudent.Count == 0)
                {
                    ViewData["result"] = "Student HAS NO Completed Courses";
                    return View();
                }
                var result = new SearchPreviousCourses
                {
                    previousCourses = previousCoursesForThisStudent
                };
                
                return View(result);
            }
            else if (SPC.ConfirmStudentSsn != null && SPC.ConfirmCoursenum != null)
            {
                if (smsContext.Courses.FirstOrDefault(x => x.Coursenum == SPC.ConfirmCoursenum) == null)
                {
                    ViewData["result"] = "Course not found";
                    return View();
                }
                else if (smsContext.Students.FirstOrDefault(x => x.Ssn == SPC.ConfirmStudentSsn) == null)
                {
                    ViewData["result"] = "Student not found";
                    return View();
                }
                else if (smsContext.PreviousCourses.FirstOrDefault(x => x.SsnNum == SPC.ConfirmStudentSsn + SPC.ConfirmCoursenum) == null)
                {
                    ViewData["result"] = "Student don't have this course ";
                    return View();
                }

                smsContext.PreviousCourses.Remove(smsContext.PreviousCourses.FirstOrDefault(x => x.SsnNum == SPC.ConfirmStudentSsn + SPC.ConfirmCoursenum));
                smsContext.SaveChanges();
                ViewData["result"] = "Course Deleted successfully";
                return View();
            }   
            else if (SPC.NewpreviousCourse.CourseNum != null && SPC.NewpreviousCourse.StudentId !=null )
            {
                if (smsContext.Courses.FirstOrDefault(x => x.Coursenum == SPC.NewpreviousCourse.CourseNum) == null)
                {
                    ViewData["result"] = "Course not found";
                    return View();
                }
                else if (smsContext.Students.FirstOrDefault(x => x.Ssn == SPC.NewpreviousCourse.StudentId) == null)
                {
                    ViewData["result"] = "Student not found";
                    return View();
                }
                else if (smsContext.PreviousCourses.FirstOrDefault(x => x.SsnNum == SPC.NewpreviousCourse.StudentId + SPC.NewpreviousCourse.CourseNum) != null)
                {
                    ViewData["result"] = "Course already exists";
                    return View();
                }
                Console.WriteLine(SPC.NewpreviousCourse.Result);
                SPC.NewpreviousCourse.SsnNum = SPC.NewpreviousCourse.StudentId + SPC.NewpreviousCourse.CourseNum;
                smsContext.PreviousCourses.Add(SPC.NewpreviousCourse);
                smsContext.SaveChanges();
                ViewData["result"] = "Course Added successfully";
                return View();
            }
            ViewData["result"] = "Nothing To DO";
            return View();
        }



        //***********************************************************************************


        [HttpGet]
        public IActionResult NewCourses()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewCourses(NewCourses NC)
        {
            if (NC.StudentSsn != null)
            {
                if (smsContext.Students.FirstOrDefault(x => x.Ssn == NC.StudentSsn) == null)
                {
                    ViewData["result"] = "Student not found";
                    return View();
                }
                var currentCoursesForThisStudent = smsContext.StudentCourses.Where(x => x.Ssn == NC.StudentSsn).ToList();
                if (currentCoursesForThisStudent.Count == 0)
                {
                    ViewData["result"] = "Student HAS NO Current Courses";
                    return View();
                }
                var result = new NewCourses
                {
                    CurrentStudentCourses = currentCoursesForThisStudent
                };

                return View(result);
            }
            else if (NC.ConfirmStudentSsn != null && NC.ConfirmCoursenum != null)
            {
                if (smsContext.Courses.FirstOrDefault(x => x.Coursenum == NC.ConfirmCoursenum) == null)
                {
                    ViewData["result"] = "Course not found";
                    return View();
                }
                else if (smsContext.Students.FirstOrDefault(x => x.Ssn == NC.ConfirmStudentSsn) == null)
                {
                    ViewData["result"] = "Student not found";
                    return View();
                }
                else if (smsContext.StudentCourses.FirstOrDefault(x => x.SsnNum == NC.ConfirmStudentSsn + NC.ConfirmCoursenum) == null)
                {
                    ViewData["result"] = "Student don't have this course ";
                    return View();
                }

                smsContext.StudentCourses.Remove(smsContext.StudentCourses.FirstOrDefault(x => x.SsnNum == NC.ConfirmStudentSsn + NC.ConfirmCoursenum));
                smsContext.SaveChanges();
                ViewData["result"] = "Course Deleted successfully";
                return View();
            }
            else if (NC.NewCourse.Coursenum != null && NC.NewCourse.Ssn != null)
            {
                if (smsContext.Courses.FirstOrDefault(x => x.Coursenum == NC.NewCourse.Coursenum) == null)
                {
                    ViewData["result"] = "Course not found";
                    return View();
                }
                else if (smsContext.Students.FirstOrDefault(x => x.Ssn == NC.NewCourse.Ssn) == null)
                {
                    ViewData["result"] = "Student not found";
                    return View();
                }
                else if  (smsContext.StudentCourses.FirstOrDefault(x => x.SsnNum == NC.NewCourse.Ssn + NC.NewCourse.Coursenum) != null)
                {
                    ViewData["result"] = "Course already exists";
                    return View();
                }
                else
                {
                    NC.NewCourse.SsnNum = NC.NewCourse.Ssn + NC.NewCourse.Coursenum;
                    smsContext.StudentCourses.Add(NC.NewCourse);
                    smsContext.SaveChanges();
                    ViewData["result"] = "Course Added successfully";
                    return View();
                }
                
            }
            ViewData["result"] = "Nothing To DO";
            return View();
        }

        //***********************************************************************************Doctor************************************************


        [HttpGet]
        public IActionResult AddDoctor()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDoctor(AddDoctor Doc)
        {
            
            var usermodel = new AppUser
            {
                UserName = Doc.Username,
                Email = Doc.Email,
                PhoneNumber = Doc.Phone,
                SSN = Doc.Ssn

            };

            var doctor = new Doctor
            {
                Fname = Doc.Fname,
                Lname = Doc.Lname,
                Username = Doc.Username,
                Email = Doc.Email,
                Phone = Doc.Phone,
                Password = Doc.Password,
                Ssn = Doc.Ssn,

            };

                

            if (smsContext.Doctors.FirstOrDefault(x => x.Ssn == Doc.Ssn) != null)
            {
                ModelState.AddModelError("SSN", "SSN already exists");
                return View(Doc);
            }
            else if (smsContext.Doctors.FirstOrDefault(x => x.Email == Doc.Email) != null)
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(Doc);
            }
            else if (smsContext.Doctors.FirstOrDefault(x => x.Username == Doc.Username) != null)
            {
                ModelState.AddModelError("Username", "Username already exists");
                return View(Doc);
            }
            else if (smsContext.Doctors.FirstOrDefault(x => x.Phone == Doc.Phone) != null)
            {
                ModelState.AddModelError("Phone", "Phone already exists");
                return View(Doc);
            }
            else
            {
                IdentityResult result = await userManager.CreateAsync(usermodel, Doc.Password);
                Console.WriteLine(result.Succeeded);
                if (result.Succeeded)
                {
                    try
                    {
                        
                        if (Doc.Image != null)
                        {
                            doctor.Id = identityContext.AspNetUsers.FirstOrDefault(x => x.Ssn == Doc.Ssn).Id;
                            smsContext.Doctors.Add(doctor);
                            smsContext.SaveChanges();
                            await userManager.AddToRoleAsync(usermodel, "Doctor");
                            Directory.CreateDirectory(Path.Combine("wwwroot", "Face ID", "Labels", Doc.Username));
                            var filestream = new FileStream(Path.Combine("wwwroot", "Face ID", "Labels", Doc.Username, 1 + ".jpg"), FileMode.Create);
                            await Doc.Image.CopyToAsync(filestream);
                            filestream.Close();
                            ViewData["result"] = "Doctor added successfully";
                            return View(Doc);
                        }

                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("Size", e.Message);
                        userManager.DeleteAsync(usermodel);
                    }
                }
                else
                {
                    smsContext.Doctors.Remove(doctor);
                    foreach (var erroritem in result.Errors)
                    {
                        ModelState.AddModelError("PassWord", erroritem.Description);
                    }
                }
            }

            ViewData["result"] = "can not add Doctor";
            return View(Doc);
        }

        //***********************************************************************************

        [HttpGet]
        public IActionResult DeleteDoctor()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDoctor(DeleteDoctor doc)
        {
            if (doc.confirmUsername != null)
            {
                var user = await userManager.FindByNameAsync(doc.confirmUsername);
                if (user == null)
                {
                    ViewData["result"] = "Username not found";
                    return View();
                }
                await userManager.DeleteAsync(user);
                System.IO.Directory.Delete(Path.Combine("wwwroot", "Face ID", "Labels", doc.confirmUsername), true);
                ViewData["result"] = "Doctor Deleted successfully";
                return View();
            }
            else if (smsContext.Doctors.FirstOrDefault(x => x.Ssn == doc.Ssn) != null)
            {

                var doctor = smsContext.Doctors.FirstOrDefault(x => x.Ssn == doc.Ssn);
                var Rdoc = new DeleteDoctor
                {
                    Ssn = doctor.Ssn,
                    Email = doctor.Email,
                    Password = doctor.Password,
                    Username = doctor.Username,
                    Fname = doctor.Fname,
                    Lname = doctor.Lname,
                    Phone = doctor.Phone,
                    Id = doctor.Id,
                };

                return View(Rdoc);
            }

            ViewData["result"] = "User not found";

            return View();

        }

        //***********************************************************************************
        [HttpGet]
        public IActionResult UpdateDoctor()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDoctor(UpdateDoctor doc)
        {

            if (doc.confirmUsername != null)
            {
                var user = await userManager.FindByNameAsync(doc.confirmUsername);
                if (user == null)
                {
                    ViewData["result"] = "Username not found";
                    return View();
                }
                else
                {

                    var doctor = smsContext.Doctors.FirstOrDefault(x => x.Username == doc.Username);
                    doctor.Fname = doc.Fname;
                    doctor.Lname = doc.Lname;
                    doctor.Phone = doc.Phone;
                    smsContext.Doctors.Update(doctor);
                    smsContext.SaveChanges();
                    ViewData["result"] = "Doctor Updated successfully";
                    return View();
                }
            }
            else if (smsContext.Doctors.FirstOrDefault(x => x.Ssn == doc.Ssn) != null)
            {

                var doctor = smsContext.Doctors.FirstOrDefault(x => x.Ssn == doc.Ssn);
                var Rdoc = new UpdateDoctor
                {
                    Ssn = doctor.Ssn,
                    Email = doctor.Email,
                    Password = doctor.Password,
                    Username = doctor.Username,
                    Fname = doctor.Fname,
                    Lname = doctor.Lname,
                    Phone = doctor.Phone,
                    Id = doctor.Id,
                };
                return View(Rdoc);
            }
            ViewData["result"] = "User not found";

            return View();
        }

        //***********************************************************************************
        [HttpGet]
        public IActionResult SearchDoctor()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchDoctor(SearchDoctor doc)
        {
            var doctor = new Doctor();
            if (smsContext.Doctors.FirstOrDefault(x => x.Ssn == doc.confirmSsn) != null)
            {
                doctor = smsContext.Doctors.FirstOrDefault(x => x.Ssn == doc.confirmSsn);

            }
            else if (smsContext.Doctors.FirstOrDefault(x => x.Phone == doc.confirmPhone) != null)
            {
                doctor = smsContext.Doctors.FirstOrDefault(x => x.Phone == doc.confirmPhone);
            }
            else if (smsContext.Doctors.FirstOrDefault(x => x.Email == doc.confirmEmail) != null)
            {
                doctor = smsContext.Doctors.FirstOrDefault(x => x.Email == doc.confirmEmail);
            }
            else if (smsContext.Doctors.FirstOrDefault(x => x.Username == doc.confirmUsername) != null)
            {
                doctor = smsContext.Doctors.FirstOrDefault(x => x.Username == doc.confirmUsername);
            }
            else
            {
                ViewData["result"] = "User not found";
                return View();
            }

            var Rdoc = new SearchDoctor();

            Rdoc.Ssn = doctor.Ssn;
            Rdoc.Email = doctor.Email;
            Rdoc.Password = doctor.Password;
            Rdoc.Username = doctor.Username;
            Rdoc.Fname = doctor.Fname;
            Rdoc.Lname = doctor.Lname;
            Rdoc.Phone = doctor.Phone;

            
            return View(Rdoc);
        }

        //***********************************************************************************

        [HttpGet]
        public IActionResult DoctorCourses()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoctorCourses(DoctorCourses DC)
        {
            if (DC.DoctorSsn != null)
            {
                if (smsContext.Doctors.FirstOrDefault(x => x.Ssn == DC.DoctorSsn) == null)
                {
                    ViewData["result"] = "Doctor not found";
                    return View();
                }
                var doctorCourses = smsContext.DocCourses.Where(x => x.Ssn == DC.DoctorSsn).ToList();
                if (doctorCourses.Count == 0)
                {
                    ViewData["result"] = "Doctor HAS NO Courses";
                    return View();
                }
                var result = new DoctorCourses
                {
                    DoctorCourse = doctorCourses
                };

                return View(result);
            }
            else if (DC.ConfirmDoctorSsn != null && DC.ConfirmCourseNum != null)
            {
                if (smsContext.Courses.FirstOrDefault(x => x.Coursenum == DC.ConfirmCourseNum) == null)
                {
                    ViewData["result"] = "Course not found";
                    return View();
                }
                else if (smsContext.Doctors.FirstOrDefault(x => x.Ssn == DC.ConfirmDoctorSsn) == null)
                {
                    ViewData["result"] = "Doctor not found";
                    return View();
                }
                else if (smsContext.DocCourses.FirstOrDefault(x => x.SsnNum == DC.ConfirmDoctorSsn + DC.ConfirmCourseNum) == null)
                {
                    ViewData["result"] = "Doctor don't have this course ";
                    return View();
                }

                smsContext.DocCourses.Remove(smsContext.DocCourses.FirstOrDefault(x => x.SsnNum == DC.ConfirmDoctorSsn + DC.ConfirmCourseNum));
                smsContext.SaveChanges();
                ViewData["result"] = "Course Deleted successfully";
                return View();
            }
            else if (DC.NewDoctorCourse.Coursenum != null && DC.NewDoctorCourse.Ssn != null)
            {
                if (smsContext.Courses.FirstOrDefault(x => x.Coursenum == DC.NewDoctorCourse.Coursenum) == null)
                {
                    ViewData["result"] = "Course not found";
                    return View();
                }
                else if (smsContext.Doctors.FirstOrDefault(x => x.Ssn == DC.NewDoctorCourse.Ssn) == null)
                {
                    ViewData["result"] = "Doctor not found";
                    return View();
                }
                else if (smsContext.DocCourses.FirstOrDefault(x => x.SsnNum == DC.NewDoctorCourse.Ssn + DC.NewDoctorCourse.Coursenum) != null)
                {
                    ViewData["result"] = "Course already exists";
                    return View();
                }
                DC.NewDoctorCourse.SsnNum = DC.NewDoctorCourse.Ssn + DC.NewDoctorCourse.Coursenum;
                smsContext.DocCourses.Add(DC.NewDoctorCourse);
                smsContext.SaveChanges();
                ViewData["result"] = "Course Added successfully";
                return View();

            }
            ViewData["result"] = "Nothing To DO";
            return View();
        }
        //***********************************************************************************Course************************************************
        [HttpGet]
        public IActionResult AddCourse()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCourse(AddCourse course)
        {
            var coursemodel = new Course
            {
                Coursename = course.Coursename,
                Coursenum = course.Coursenum,
                CreditHuors = course.CreditHuors,
                CourseLevel = course.level,
                CourseDescription = course.CourseDescription
            };



            if (smsContext.Courses.FirstOrDefault(x => x.Coursenum == course.Coursenum) != null)
            {
                ModelState.AddModelError("Coursenum", "Coursenum already exists");
                return View(course);
            }
            else if (smsContext.Courses.FirstOrDefault(x => x.Coursename == course.Coursename) != null)
            {
                ModelState.AddModelError("Coursename", "Coursename already exists");
                return View(course);
            }
            else
            {
                try
                {
                    var cover = Path.Combine("wwwroot", "Books", course.Coursenum, "Cover");
                    var book = Path.Combine("wwwroot", "Books", course.Coursenum, "book");
                    var lectures = Path.Combine("wwwroot", "Books", course.Coursenum, "lectures");
                    var assignments = Path.Combine("wwwroot", "Books", course.Coursenum, "Assignments");

                    smsContext.Courses.Add(coursemodel);
                    smsContext.SaveChanges();

                    Directory.CreateDirectory(cover);
                    Directory.CreateDirectory(book);
                    Directory.CreateDirectory(lectures);
                    Directory.CreateDirectory(assignments);


                    var filestream = new FileStream(Path.Combine(cover , course.Coursenum + ".jpg")  , FileMode.Create );
                    await course.CourseCover.CopyToAsync(filestream);
                    filestream.Close();

                    

                    ViewData["result"] = "Course added successfully";
                    return View(course);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Size", e.Message);
                }
            }
            ViewData["result"] = "can not add Course";
            return View();


        }

        //***********************************************************************************
        [HttpGet]
        public IActionResult DeleteCourse()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourse(DeleteCourse course)
        {
            if (course.confirmCoursename != null)
            {
                var coursemodel = smsContext.Courses.FirstOrDefault(x => x.Coursename == course.confirmCoursename);
                if (coursemodel == null)
                {
                    ViewData["result"] = "Course not found";
                    return View();
                }
                smsContext.Courses.Remove(coursemodel);
                smsContext.SaveChanges();
                if (Directory.Exists(Path.Combine("wwwroot", "Books", coursemodel.Coursenum)))
                    System.IO.Directory.Delete(Path.Combine("wwwroot", "Books", coursemodel.Coursenum), true);
                ViewData["result"] = "Course Deleted successfully";
                return View();
            }
            else if (smsContext.Courses.FirstOrDefault(x => x.Coursenum == course.Coursenum) != null)
            {

                var coursemodel = smsContext.Courses.FirstOrDefault(x => x.Coursenum == course.Coursenum);
                var Rcourse = new DeleteCourse
                {
                    Coursenum = coursemodel.Coursenum,
                    Coursename = coursemodel.Coursename,
                    CreditHuors = coursemodel.CreditHuors,
                    level = coursemodel.CourseLevel,
                    CourseDescription = coursemodel.CourseDescription
                };

                return View(Rcourse);
            }

            ViewData["result"] = "Course not found";

            return View();

        }

        //***********************************************************************************

        [HttpGet]
        public IActionResult SearchCourse()
        {
            return View();
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchCourse(SearchCourse course)
        {
            var list = new List<Course>();
            var Rcourse = new SearchCourse();
            if (smsContext.Courses.FirstOrDefault(x => x.Coursenum == course.confirmCoursenum) != null)
            {
                list.Add(smsContext.Courses.FirstOrDefault(x => x.Coursenum == course.confirmCoursenum));

            }
            else if (smsContext.Courses.FirstOrDefault(x => x.Coursename == course.confirmCoursename) != null)
            {
                list.Add(smsContext.Courses.FirstOrDefault(x => x.Coursename == course.confirmCoursename));
            }
            else if (smsContext.Courses.FirstOrDefault(x => x.CourseLevel == course.confirmLevel) != null)
            {
                list = smsContext.Courses.Where(x => x.CourseLevel == course.confirmLevel).ToList();
            }
            else if (smsContext.Courses.FirstOrDefault(x => x.CreditHuors.ToString() == course.confirmCreditHours) != null)
            {
                list = smsContext.Courses.Where(x => x.CreditHuors.ToString() == course.confirmCreditHours).ToList();
            }
            else
            {
                ViewData["result"] = "Course not found";
                return View();
            }
            Rcourse.Courses = list;           

            return View(Rcourse);
        }




        
    }

}










   

