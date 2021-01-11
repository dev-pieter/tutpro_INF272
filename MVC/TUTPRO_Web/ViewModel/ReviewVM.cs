using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TUTPRO_Web.models;

namespace TUTPRO_Web.ViewModel
{
    public class ReviewVM
    {
        public string StarRating { get; set; }
        public DateTime ReviewDate { get; set; }
        public string Message { get; set; }
        public SelectList TutorList { get; set; }

        public int TutorID  {get;set;}
    }
}