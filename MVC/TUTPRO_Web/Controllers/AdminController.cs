using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TUTPRO_Web.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult Reports()
        {
            return View();
        }

        public ActionResult Database()
        {
            return View();
        }

        public ActionResult tutorApplications()
        {
            return View();
        }
    }
}