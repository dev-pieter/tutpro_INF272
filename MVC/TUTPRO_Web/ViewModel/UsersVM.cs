using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TUTPRO_Web.models;
using System.Web.Mvc;
namespace TUTPRO_Web.ViewModel
{
    public class UsersVM
    {
        public User user { get; set; }
        public List<User> users { get; set; }

        public SelectList UserType { get; set; }


    }
}