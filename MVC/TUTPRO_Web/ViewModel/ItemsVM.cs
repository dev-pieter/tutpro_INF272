using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TUTPRO_Web.models;
using System.Web.Mvc;
namespace TUTPRO_Web.ViewModel
{
    public class ItemsVM
    {
        public string StudentName { get; set; }

        public Admin admin { get; set; }
        public Student student { get; set; }
        public Tutor tutor { get; set; }

        public List<UserVM> users { get; set; }


    }
}