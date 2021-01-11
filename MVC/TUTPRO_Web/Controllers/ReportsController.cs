using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;

namespace TUTPRO_Web.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports

        public ActionResult EvaluationReport()
        {
            return View();
        }

        public ActionResult MonthlyReport()
        {
            return View();
        }

        public ActionResult SessionsReport()
        {
            return View();
        }

        public ActionResult ExportMonthly()
        {
            return new ActionAsPdf("MonthlyReport")
            {
                FileName = Server.MapPath("~/Content/MonthlyReport.pdf")
            };
        }
        public ActionResult ExportEvaluation()
        {
            return new ActionAsPdf("EvaluationReport")
            {
                FileName = Server.MapPath("~/Content/EvaluationReport.pdf")
            };
        }
        public ActionResult ExportSession()
        {
            return new ActionAsPdf("SessionsReport")
            {
                FileName = Server.MapPath("~/Content/SessionsReport.pdf")
            };
        }
    }
}