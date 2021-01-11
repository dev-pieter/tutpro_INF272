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
    public class TutorController : Controller
    {
        private static readonly TUTPROEntities db = new TUTPROEntities();
        // GET: Tutor
        public ActionResult Home()
        {

            return View();
        }

        public ActionResult Profile()
        {
            int userID = Convert.ToInt32(HttpContext.Session["UserID"]);
            var TutorProfile = db.Tutors.Include(o => o.User).Where(o => o.User_ID == userID).ToList();
            return View(TutorProfile);
        }

        public ActionResult MyReview(int id)
        {
            var ViewReviews = db.Reviews.Include(o => o.Tutor).Where(o => o.Tutor_ID == id).ToList();
            return View(ViewReviews);
        }

        public ActionResult About()
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
                if (file == null)
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

        public ActionResult logSession()
        {
            return View();
        }

        public ActionResult LogSuccess()
        {
            return View();
        }
    }
}