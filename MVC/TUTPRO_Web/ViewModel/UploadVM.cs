using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TUTPRO_Web.ViewModel
{
    public class UploadVM
    {
        public HttpPostedFileBase[] uploadFile { get; set; }
        public string Upload { get; set; }
        public string Username { get; set; }
    }
}