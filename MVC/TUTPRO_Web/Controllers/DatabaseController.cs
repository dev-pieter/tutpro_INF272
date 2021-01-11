using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TUTPRO_Web.models;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Data.Entity.Migrations;

namespace TUTPRO_Web.Controllers
{
    public class DatabaseController : Controller
    {
        private static readonly TUTPROEntities db = new TUTPROEntities();
        private static string SelectedDatabase;
        static int edittypeid;


        // GET: Database
        public ActionResult GetSelectedDatabase()
        {
            SelectedDatabase = Request.Form["Database"].ToString();
            return RedirectToAction(SelectedDatabase + "Index");
        }

        public ActionResult UserIndex()
        {
            var users = db.Users.Include(u => u.UserType);
            return View(users.ToList());
        }

        public ActionResult UserCreate()
        {
            ViewBag.UserType_ID = new SelectList(db.UserTypes, "UserType_ID", "UserDescription");
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        public ActionResult UserCreate([Bind(Include = "User_ID,UserType_ID,Name,Surname,Email,Username,Password,PhoneNo")] User user)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    if (user.UserType_ID == 1)
                    {
                        Admin admin = new Admin();
                        admin.User_ID = user.User_ID;
                        db.Admins.Add(admin);

                    }
                    else if (user.UserType_ID == 2)
                    {
                        Tutor tutor = new Tutor();
                        tutor.User_ID = user.User_ID;
                        db.Tutors.Add(tutor);
                    }
                    else if (user.UserType_ID == 3)
                    {
                        //if ()
                        //{
                            Student student = new Student();
                            student.User_ID = user.User_ID;
                            db.Students.Add(student);
                        //}
                        //else {
                        //    Student student = new Student();
                        //    student.User_ID = user.User_ID;
                        //    db.Students.Add(student);
                        //}
                       
                    }

                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("UserIndex");
                }
            }

            ViewBag.UserType_ID = new SelectList(db.UserTypes, "UserType_ID", "UserDescription", user.UserType_ID);
            return View(user);
        }

        // GET: Users/Edit
        public ActionResult UserEdit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            edittypeid = user.UserType_ID;
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserType_ID = new SelectList(db.UserTypes, "UserType_ID", "UserDescription", user.UserType_ID);
            return View(user);
        }

        // POST: Users/Edit
        [HttpPost]
        public ActionResult UserEdit([Bind(Include = "User_ID,Name,Surname,Email,Username,Password,PhoneNo")] User user)
        {
            user.UserType_ID = edittypeid;
            db.Users.AddOrUpdate(user);
                db.SaveChanges();
                return RedirectToAction("UserIndex");

        }

        // GET: Users/Delete
        public ActionResult UserDelete(int? id)
        {
           

            User user = db.Users.Find(id);
            db.Users.Remove(user);                    
            //Student studenttoDelete = db.Students.Where(o => o.User_ID == id).FirstOrDefault();

            //Student Find = db.Students.Find(studenttoDelete);
            //HighSchoolStudent high = db.HighSchoolStudents.Where(o => o.HighSchoolStudent_ID == st).FirstOrDefault();
            //db.Students.Remove(studenttoDelete);
          
            db.SaveChanges();
            return RedirectToAction("UserIndex");
        }

        // GET: Applications
        public ActionResult ApplicationIndex()
        {
            var applications = db.Applications.Include(a => a.User);
            return View(applications.ToList());
        }

        // GET: Applications/Create
        public ActionResult ApplicationCreate()
        {
            ViewBag.User_ID = new SelectList(db.Users, "User_ID", "Name");
            return View();
        }

        // POST: Applications/Create
        [HttpPost]
        public ActionResult ApplicationCreate([Bind(Include = "Application_ID,Status,User_ID")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Applications.Add(application);
                db.SaveChanges();
                return RedirectToAction("ApplicationIndex");
            }

            ViewBag.User_ID = new SelectList(db.Users, "User_ID", "Name", application.User_ID);
            return View(application);
        }
        // GET: Applications/Edit
        public ActionResult ApplicationEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_ID = new SelectList(db.Users, "User_ID", "Name", application.User_ID);
            return View(application);
        }

        // POST: Applications/Edit
        [HttpPost]
        public ActionResult ApplicationEdit([Bind(Include = "Application_ID,Status,User_ID")] Application application)
        {
            if (ModelState.IsValid)
            {
                db.Entry(application).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ApplicationIndex");
            }
            ViewBag.User_ID = new SelectList(db.Users, "User_ID", "Name", application.User_ID);
            return View(application);
        }

        // GET: Applications/Delete
        public ActionResult ApplicationDelete(int? id)
        {
            Application application = db.Applications.Find(id);
            db.Applications.Remove(application);
            db.SaveChanges();
            return RedirectToAction("ApplicationIndex");
        }

        // GET: Reviews
        public ActionResult ReviewIndex()
        {
            var reviews = db.Reviews.Include(r => r.Tutor).Include(r => r.Tutor.User);
            return View(reviews.ToList());
        }

        // GET: Reviews/Create
        public ActionResult ReviewCreate()
        {
            ViewBag.Tutor_ID = new SelectList(db.Tutors, "Tutor_ID", "Tutor_ID");
            return View();
        }

        // POST: Reviews/Create
        [HttpPost]
        public ActionResult ReviewCreate([Bind(Include = "Review_ID,Tutor_ID,StarRating,ReviewDate,Message")] Review review)
        {
            if (ModelState.IsValid)
            {
               
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("ReviewIndex");
            }

            ViewBag.Tutor_ID = new SelectList(db.Tutors, "Tutor_ID", "Tutor_ID", review.Tutor_ID);
            return View(review);
        }

        // GET: Reviews/Edit
        public ActionResult ReviewEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            ViewBag.Tutor_ID = new SelectList(db.Tutors, "Tutor_ID", "Tutor_ID", review.Tutor_ID);
            return View(review);
        }

        // POST: Reviews/Edit
        [HttpPost]
        public ActionResult ReviewEdit([Bind(Include = "Review_ID,Tutor_ID,StarRating,ReviewDate,Message")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ReviewIndex");
            }
            ViewBag.Tutor_ID = new SelectList(db.Tutors, "Tutor_ID", "Tutor_ID", review.Tutor_ID);
            return View(review);
        }

        public ActionResult ReviewDelete(int? id)
        {
            Review review = db.Reviews.Find(id);
            db.Reviews.Remove(review);
            db.SaveChanges();
            return RedirectToAction("ReviewIndex");
        }

        // GET: Demerits
        public ActionResult DemeritIndex()
        {
            var demerits = db.Demerits.Include(d => d.Student).Include(d => d.Student.User);
            return View(demerits.ToList());
        }

        // GET: Demerits/Create
        public ActionResult DemeritCreate()
        {
            ViewBag.Student_ID = new SelectList(db.Students, "Student_ID", "Student_ID");
            return View();
        }

        // POST: Demerits/Create
        [HttpPost]
        public ActionResult DemeritCreate([Bind(Include = "Demerit1,Student_ID,DemeritDescription,DemeritDate")] Demerit demerit)
        {
            if (ModelState.IsValid)
            {
                db.Demerits.Add(demerit);
                db.SaveChanges();
                return RedirectToAction("DemeritIndex");
            }

            ViewBag.Student_ID = new SelectList(db.Students, "Student_ID", "Student_ID", demerit.Student_ID);
            return View(demerit);
        }

        // GET: Demerits/Edit
        public ActionResult DemeritEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Demerit demerit = db.Demerits.Find(id);
            if (demerit == null)
            {
                return HttpNotFound();
            }
            ViewBag.Student_ID = new SelectList(db.Students, "Student_ID", "Student_ID", demerit.Student_ID);
            return View(demerit);
        }

        // POST: Demerits/Edit
        [HttpPost]
        public ActionResult DemeritEdit([Bind(Include = "Demerit1,Student_ID,DemeritDescription,DemeritDate")] Demerit demerit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(demerit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DemeritIndex");
            }
            ViewBag.Student_ID = new SelectList(db.Students, "Student_ID", "Student_ID", demerit.Student_ID);
            return View(demerit);
        }

        // GET: Demerits/Delete
        public ActionResult DemeritDelete(int? id)
        {
            Demerit demerit = db.Demerits.Find(id);
            db.Demerits.Remove(demerit);
            db.SaveChanges();
            return RedirectToAction("DemeritIndex");
        }

        // GET: HighSchoolStudents
        public ActionResult HighSchoolIndex()
        {
            var highSchoolStudents = db.HighSchoolStudents.Include(h => h.Grade).Include(h => h.Student.User);
            return View(highSchoolStudents.ToList());
        }

        // GET: HighSchoolStudents/Create
        public ActionResult HighSchoolCreate()
        {
            ViewBag.Grade_ID = new SelectList(db.Grades, "Grade_ID", "GradeName");
            ViewBag.Student_ID = new SelectList(db.Students, "Student_ID", "Student_ID");
            return View();
        }

        // POST: HighSchoolStudents/Create
        [HttpPost]
        public ActionResult HighSchoolCreate([Bind(Include = "HighSchoolStudent_ID,Grade_ID,Student_ID")] HighSchoolStudent highSchoolStudent)
        {
            if (ModelState.IsValid)
            {
                db.HighSchoolStudents.Add(highSchoolStudent);
                db.SaveChanges();
                return RedirectToAction("HighSchoolIndex");
            }

            ViewBag.Grade_ID = new SelectList(db.Grades, "Grade_ID", "GradeName", highSchoolStudent.Grade_ID);
            ViewBag.Student_ID = new SelectList(db.Students, "Student_ID", "Student_ID", highSchoolStudent.Student_ID);
            return View(highSchoolStudent);
        }

        // GET: HighSchoolStudents/Edit
        public ActionResult HighSchoolEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HighSchoolStudent highSchoolStudent = db.HighSchoolStudents.Find(id);
            if (highSchoolStudent == null)
            {
                return HttpNotFound();
            }
            ViewBag.Grade_ID = new SelectList(db.Grades, "Grade_ID", "GradeName", highSchoolStudent.Grade_ID);
            ViewBag.Student_ID = new SelectList(db.Students, "Student_ID", "Student_ID", highSchoolStudent.Student_ID);
            return View(highSchoolStudent);
        }

        // POST: HighSchoolStudents/Edit
        [HttpPost]
        public ActionResult HighSchoolEdit([Bind(Include = "HighSchoolStudent_ID,Grade_ID,Student_ID")] HighSchoolStudent highSchoolStudent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(highSchoolStudent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("HighSchoolIndex");
            }
            ViewBag.Grade_ID = new SelectList(db.Grades, "Grade_ID", "GradeName", highSchoolStudent.Grade_ID);
            ViewBag.Student_ID = new SelectList(db.Students, "Student_ID", "Student_ID", highSchoolStudent.Student_ID);
            return View(highSchoolStudent);
        }

        // GET: HighSchoolStudents/Delete
        public ActionResult HighSchoolDelete(int? id)
        {
            HighSchoolStudent highSchoolStudent = db.HighSchoolStudents.Find(id);
            db.HighSchoolStudents.Remove(highSchoolStudent);
            db.SaveChanges();
            return RedirectToAction("HighSchoolIndex");
        }

        // GET: UniversityStudents
        public ActionResult UniversityIndex()
        {
            var universityStudents = db.UniversityStudents.Include(u => u.Degree).Include(u => u.Student.User);
            return View(universityStudents.ToList());
        }

        // GET: UniversityStudents/Create
        public ActionResult UniversityCreate()
        {
            ViewBag.Degree_ID = new SelectList(db.Degrees, "Degree_ID", "DegreeName");
            
            ViewBag.Student_ID = new SelectList(db.Students, "Student_ID", "Student_ID");
            return View();
        }

        // POST: UniversityStudents/Create
        [HttpPost]
        public ActionResult UniversityCreate([Bind(Include = "University_Account,Student_ID,Degree_ID,Enrollment_ID")] UniversityStudent universityStudent)
        {
            if (ModelState.IsValid)
            {
                db.UniversityStudents.Add(universityStudent);
                db.SaveChanges();
                return RedirectToAction("UniversityIndex");
            }

            ViewBag.Degree_ID = new SelectList(db.Degrees, "Degree_ID", "DegreeName", universityStudent.Degree_ID);
            
            ViewBag.Student_ID = new SelectList(db.Students, "Student_ID", "Student_ID", universityStudent.Student_ID);
            return View(universityStudent);
        }

        // GET: UniversityStudents/Edit
        public ActionResult UniversityEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UniversityStudent universityStudent = db.UniversityStudents.Find(id);
            if (universityStudent == null)
            {
                return HttpNotFound();
            }
            ViewBag.Degree_ID = new SelectList(db.Degrees, "Degree_ID", "DegreeName", universityStudent.Degree_ID);
            
            ViewBag.Student_ID = new SelectList(db.Students, "Student_ID", "Student_ID", universityStudent.Student_ID);
            return View(universityStudent);
        }

        // POST: UniversityStudents/Edit
        [HttpPost]
        public ActionResult UniversityEdit([Bind(Include = "University_Account,Student_ID,Degree_ID,Enrollment_ID")] UniversityStudent universityStudent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(universityStudent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("UniversityIndex");
            }
            ViewBag.Degree_ID = new SelectList(db.Degrees, "Degree_ID", "DegreeName", universityStudent.Degree_ID);
           
            ViewBag.Student_ID = new SelectList(db.Students, "Student_ID", "Student_ID", universityStudent.Student_ID);
            return View(universityStudent);
        }

        // GET: UniversityStudents/Delete
        public ActionResult UniversityDelete(int? id)
        {
            UniversityStudent universityStudent = db.UniversityStudents.Find(id);
            db.UniversityStudents.Remove(universityStudent);
            db.SaveChanges();
            return RedirectToAction("UniversityIndex");
        }

        // GET: Resources
        public ActionResult ResourceIndex()
        {
            var resources = db.Resources.Include(r => r.Module).Include(r => r.Subject);
            return View(resources.ToList());
        }

        // GET: Resources/Create
        public ActionResult ResourceCreate()
        {
            ViewBag.Module_ID = new SelectList(db.Modules, "Module_ID", "ModuleCode");
            ViewBag.Subject_ID = new SelectList(db.Subjects, "Subject_ID", "SubjectName");
            return View();
        }

        // POST: Resources/Create
        [HttpPost]
        public ActionResult ResourceCreate([Bind(Include = "Resource_ID,Module_ID,Subject_ID,UploadDate,FileName")] Resource resource)
        {
            if (ModelState.IsValid)
            {
                db.Resources.Add(resource);
                db.SaveChanges();
                return RedirectToAction("ResourceIndex");
            }

            ViewBag.Module_ID = new SelectList(db.Modules, "Module_ID", "ModuleCode", resource.Module_ID);
            ViewBag.Subject_ID = new SelectList(db.Subjects, "Subject_ID", "SubjectName", resource.Subject_ID);
            return View(resource);
        }

        // GET: Resources/Edit
        public ActionResult ResourceEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resource resource = db.Resources.Find(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            ViewBag.Module_ID = new SelectList(db.Modules, "Module_ID", "ModuleCode", resource.Module_ID);
            ViewBag.Subject_ID = new SelectList(db.Subjects, "Subject_ID", "SubjectName", resource.Subject_ID);
            return View(resource);
        }

        // POST: Resources/Edit
        [HttpPost]
        public ActionResult ResourceEdit([Bind(Include = "Resource_ID,Module_ID,Subject_ID,UploadDate,FileName")] Resource resource)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resource).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ResourceIndex");
            }
            ViewBag.Module_ID = new SelectList(db.Modules, "Module_ID", "ModuleCode", resource.Module_ID);
            ViewBag.Subject_ID = new SelectList(db.Subjects, "Subject_ID", "SubjectName", resource.Subject_ID);
            return View(resource);
        }

        // GET: Resources/Delete
        public ActionResult ResourceDelete(int? id)
        {
            Resource resource = db.Resources.Find(id);
            db.Resources.Remove(resource);
            db.SaveChanges();
            return RedirectToAction("ResourceIndex");
        }

        // GET: Admins
        public ActionResult AdminIndex()
        {
            var admins = db.Admins.Include(a => a.User);
            return View(admins.ToList());
        }

        // GET: Admins/Create
        public ActionResult AdminCreate()
        {
            ViewBag.User_ID = new SelectList(db.Users, "User_ID", "Name");
            return View();
        }

        // POST: Admins/Create
        [HttpPost]
        public ActionResult AdminCreate([Bind(Include = "Admin_ID,User_ID")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("AdminIndex");
            }

            ViewBag.User_ID = new SelectList(db.Users, "User_ID", "Name", admin.User_ID);
            return View(admin);
        }

        // GET: Admins/Delete
        public ActionResult AdminDelete(int? id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("AdminIndex");
        }


    }
}