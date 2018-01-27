using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSD.Web.Areas.ContentManagement.Models
{
    public class UpdateHtmlContentViewModel
    {
        [AllowHtml]
        public string Content { get; set; }
    }
}