using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TUTPRO_Web.models;
using TUTPRO_Web.ViewModel;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Data.Entity.Migrations;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Data.Entity.Validation;

namespace TUTPRO_Web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly TUTPROEntities db = new TUTPROEntities();
        private static List<UserVM> userslist;
    

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult User(string username, string password)
        {
            string pass = password;
            var hash = GenerateHash(ApplySomeSalt(pass));


            User user = db.Users.Where(p => p.Username == username && p.Password == hash).FirstOrDefault();

            if (user != null)
            {
                HttpContext.Session["userType"] = user.UserType_ID;
                HttpContext.Session["userID"] = user.User_ID;

                if (user.UserType_ID == 1)
                {
                    return RedirectToAction("Admin", "Admin");
                }
                else if (user.UserType_ID == 2)
                {
                    return RedirectToAction("Home", "Tutor");
                }
                else if (user.UserType_ID == 3)
                {
                    return RedirectToAction("Home", "Student");
                }
            }
                    
            return RedirectToAction("Fail", "Home");
            
        }

        public ActionResult Fail()
        {

            return View();
        }


        private SelectList DegreeSelectList()
        {
            SelectList d = new SelectList(db.Degrees.ToList(), "Degree_ID", "DegreeName");
            return d;
        }
  
        private SelectList GradeSelectList()
        {
            SelectList g = new SelectList(db.Grades.ToList(), "Grade_ID", "GradeName");
            return g;
        }
   
        private SelectList UserTypeSelectList()
        {
            SelectList u = new SelectList(db.UserTypes.Where(o => o.UserType_ID == 2 || o.UserType_ID == 3).ToList(), "UserType_ID", "UserDescription");
            return u;
        }

      
        public ActionResult Register()
        {
            //userslist = new List<UserVM>();

            Register register = new Register
            {
                DegreeSelectList = DegreeSelectList(),
                
                GradeSelectList = GradeSelectList(),
                               
            };
            return View(register);
        }



        public dynamic RegisterUser(string name, string surname, string email, string username, string password, string confirmpassword, string phonenumber, int grade, int degree, int studentType)
        {

            string test = "PLEASE Workkk";

          try
            {
                //add user
                User user = new User();
                user.UserType_ID = 3;
                user.Name = name;
                user.Surname = surname;
                user.Email = email;
                user.Username = username;

                //hashing of password
                string pass = password;
                var hash = GenerateHash(ApplySomeSalt(pass));               
                user.Password = hash;   

                user.PhoneNo = phonenumber;
                db.Users.Add(user);

                //add student
                Student newStudent = new Student();
                newStudent.User_ID = user.User_ID;
                newStudent.StudentType_ID = studentType;
                db.Students.Add(newStudent);

                //highschool student
                if (studentType == 1)
                {
                    HighSchoolStudent hstudent = new HighSchoolStudent();
                    hstudent.Student_ID = newStudent.Student_ID;
                    hstudent.Grade_ID = Convert.ToInt32(grade);
                    db.HighSchoolStudents.Add(hstudent);

                }
                //university student
                else if (studentType == 2)
                {
                    UniversityStudent ustudent = new UniversityStudent();
                    ustudent.Student_ID = newStudent.Student_ID;
                    ustudent.Degree_ID = Convert.ToInt32(degree);
                    db.UniversityStudents.Add(ustudent);
                }
                db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            return test;
        }

        //hashingggg

        public static string ApplySomeSalt(string input)
        {
            try
            {
                return input + "hijsbdlhishvhisabhbhahvbchlsbv";
            }
            catch
            {
                return null;
            }
        }

        public static string GenerateHash(string Inputstring)
        {
            try
            {
                SHA256 sha256 = SHA256Managed.Create();
                byte[] bytes = Encoding.UTF8.GetBytes(Inputstring);
                byte[] hash = sha256.ComputeHash(bytes);
                return GetStringFromHash(hash);
            }
            catch
            {
                return null;
            }

        }


        private static string GetStringFromHash(byte[] hash)
        {
            try
            {
                StringBuilder result = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    result.Append(hash[i].ToString("X2"));
                }
                return result.ToString();
            }
            catch
            {
                return null;
            }
        }

        public ActionResult TutorValidation()
        {

            return View("TutorValidation");
        }

        public ActionResult ForgotPassword()
        {

            return View();
        }

        public ActionResult RegisterTutor()
        {
            TutorVM members = new TutorVM();
            return View("RegisterTutor", members);
        }

        [HttpPost]
        public ActionResult RegisterTutor(ViewModel.TutorVM members)
        {

            /* string fileName = Path.GetFileNameWithoutExtension(members.File);
             string fileExtention = Path.GetFileNameWithoutExtension(members.File);*/


            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("No input");
                return View(members);
            }
            else
            {
                User user = new User();
                user.UserType_ID = 2;
                user.Name = members.Name;
                user.Surname = members.Surname;
                user.Email = members.Email;
                user.Username = members.Username;
                user.Password = members.Password;
                user.PhoneNo = members.Number;

                User check = db.Users.Where(p => p.Username == members.Username).FirstOrDefault();

                if (check != null)
                {
                    ModelState.AddModelError("Username", "Username already exists!");
                    return View(members);
                }

                string dateTime = System.DateTime.Now.ToString("ddMMyyyHHMMss");

                foreach (HttpPostedFileBase file in members.uploadFile)
                {
                    if (file == null)
                    {
                        ModelState.AddModelError("uploadFile", "Please select a file!");
                        return View(members);
                    }
                    else
                    {
                        string extension = Path.GetExtension(file.FileName);

                        if (extension == ".pdf")
                        {
                            string finalPath = "\\UploadedFiles\\Applications\\" + members.Username + " - " + Path.GetFileName(file.FileName);
                            file.SaveAs(Server.MapPath("~") + finalPath);
                            members.Upload = finalPath;
                        }
                        else
                        {
                            ModelState.AddModelError("uploadFile", "PDF ONLY!");
                            return View(members);
                        }
                    }     

                }

                Application app = new Application();

                app.Status = "Pending";
                app.User_ID = user.User_ID;

                db.Users.Add(user);
                db.Applications.Add(app);

                //Value Tests
                System.Diagnostics.Debug.WriteLine(user.Name);
                System.Diagnostics.Debug.WriteLine(user.Surname);
                System.Diagnostics.Debug.WriteLine(user.Email);
                System.Diagnostics.Debug.WriteLine(user.Username);
                System.Diagnostics.Debug.WriteLine(user.Password);
                System.Diagnostics.Debug.WriteLine(user.PhoneNo);

                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                return RedirectToAction("TutorValidation");
            }


        }

    }
}