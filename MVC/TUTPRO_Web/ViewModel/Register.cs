using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TUTPRO_Web.models;

namespace TUTPRO_Web.ViewModel
{
    public class Register
    {
        public SelectList DegreeSelectList { get; set; }
        public Degree degree { get; set; }

        public SelectList GradeSelectList { get; set; }
        public Grade grade { get; set; }


        public SelectList UserTypeSelectList { get; set; }
        public UserType userType { get; set; }
    }
}