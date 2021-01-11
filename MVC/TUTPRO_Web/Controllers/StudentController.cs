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

namespace TUTPRO_Web.Controllers
{
    public class StudentController : Controller
    {
        private static readonly TUTPROEntities db = new TUTPROEntities();
        private int userId;
        // GET: Student
        public ActionResult Home()
        {

            return View();
        }
        public ActionResult Profile()
        {
            userId = Convert.ToInt32(HttpContext.Session["UserID"]);
            var StudentProfile = db.Students.Include(o => o.User).Where(o => o.User_ID == userId).Include(o => o.StudentType).ToList();
            return View(StudentProfile);
        }

        public ActionResult MyDemerit(int id)
        {
            var ViewDemerits = db.Demerits.Include(o => o.Student).Where(o => o.Student_ID == id).ToList();
            return View(ViewDemerits);
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Booking()
        {

            return View();
        }
        public ActionResult ChangesMade()
        {

            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult googleDrive()
        {
            UploadVM member = new UploadVM();
            return View("googleDrive", member);
        }

        [HttpPost]
        public ActionResult googleDrive(UploadVM member)
        {
            int userID = Convert.ToInt32(HttpContext.Session["UserID"]);
            User user = db.Users.Where(p => p.User_ID == userID).FirstOrDefault();

            if (user == null)
            {
                ModelState.AddModelError("uploadFile", "You must be Logged in to do that!");
                return View(member);
            }

            foreach (HttpPostedFileBase file in member.uploadFile)
            {
                if(file == null)
                {
                    ModelState.AddModelError("uploadFile", "Please select a file.");
                    return View(member);
                }
                string extension = Path.GetExtension(file.FileName);

                if (extension == ".pdf")
                {
                    string finalPath = "\\UploadedFiles\\Resources\\" + user.Username + " - " + Path.GetFileName(file.FileName);
                    file.SaveAs(Server.MapPath("~") + finalPath);
                    member.Upload = finalPath;
                }
                else
                {
                    ModelState.AddModelError("uploadFile", "PDF ONLY! Thank you.");
                    return View(member);
                }
            }

            return RedirectToAction("ThankyouUpload");
        }

        public ActionResult RegisterSuccess()
        {

            return View();
        }

        public ActionResult FindTutor()
        {
            var Findtutor = db.Tutors.Include(o => o.User);
            return View(Findtutor.ToList());
        }

        public ActionResult TutorReview(int id)
        {
            var TutorReviews = db.Reviews.Where(o => o.Tutor_ID == id);
            return View(TutorReviews.ToList());
        }

        public ActionResult ThankyouUpload()
        {

            return View();
        }

        public ActionResult UpdateProfile()
        {

            return View();
        }

        public ActionResult ReviewThankYou()
        {

            return View();
        }
        public ActionResult ReviewTutor(ViewModel.ReviewVM members)
        {

            ReviewVM tutor = new ReviewVM
            {
                TutorList = TutorSelectList()
            };
         
            Review review = new Review();

            var test = Convert.ToInt32(members.TutorID);
        
            review.StarRating = members.StarRating;
            review.ReviewDate = DateTime.Now;
            review.Message = members.Message;

            if (string.IsNullOrEmpty(review.StarRating))
            {
                System.Diagnostics.Debug.WriteLine("No input");
                return View(tutor);
            }
            else
            {

                var tutorID = db.Tutors.Where(o => o.User_ID == test).FirstOrDefault().Tutor_ID;
                var ID = Convert.ToInt32(tutorID);
                review.Tutor_ID = ID;
                db.Reviews.Add(review);
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
                return RedirectToAction("ReviewThankYou", "Student");
            }

        }

        private SelectList TutorSelectList()
        {
            SelectList u = new SelectList(db.Users.Where(n => n.UserType_ID ==2).ToList(),"User_ID","Name");
            return u;
        }

        private SelectList DegreeSelectList()
        {
            SelectList d = new SelectList(db.Degrees.ToList(), "Degree_ID", "DegreeName");
            return d;
        }
    }
}