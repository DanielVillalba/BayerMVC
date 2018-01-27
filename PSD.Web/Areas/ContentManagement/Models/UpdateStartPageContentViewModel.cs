using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSD.Web.Areas.ContentManagement.Models
{
    public class UpdateStartPageContentViewModel
    {
        [AllowHtml]
        public string Title { get; set; }
        [AllowHtml]
        public string Subtitle { get; set; }
        [AllowHtml]
        public string Paragraph { get; set; }
        [AllowHtml]
        public string Button { get; set; }
    }
}