using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSD.Web.Models
{
    public class UserLoginModel
    {
        public string NickName { get; set; }
        public string Password { get; set; }
        public bool KeepSession { get; set; }

    }
}