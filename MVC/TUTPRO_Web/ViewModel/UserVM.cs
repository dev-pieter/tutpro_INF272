using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TUTPRO_Web.models;

namespace TUTPRO_Web.ViewModel
{
    public class UserVM
    {
        public User User { get; set; } 
        public List<UserVM> Userlist { get; set; }
        public int UserType_ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PhoneNo { get; set; }
        public int student_ID { get; set; }
        
        public int tutor_ID { get; set; }
        


    }
}