using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PSD.Web.Areas.Content.Models
{
    public class ContactPageViewModel
    {
        public string Name { get; set; }
        public string EMail { get; set; }
        public string Message { get; set; }
    }
}