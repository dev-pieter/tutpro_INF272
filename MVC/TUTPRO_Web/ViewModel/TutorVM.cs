using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TUTPRO_Web.ViewModel
{
    public class TutorVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Number { get; set; }

        [Required]
        public HttpPostedFileBase[] uploadFile { get; set; }

        public string Upload { get; set; }

    }
}