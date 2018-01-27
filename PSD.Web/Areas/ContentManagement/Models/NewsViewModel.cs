using System;
using System.Web;
using System.Web.Mvc;

namespace PSD.Web.Areas.ContentManagement.Models
{
    public class NewsViewModel
    {
        public int Id { get; set; }
        public string URLId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        [AllowHtml]
        public string Paragraph { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public bool IsImageDirty { get; set; }
        public string ImageName { get; set; }
        public string ImageFooter { get; set; }
        public string Author { get; set; }
        public bool IsDistributorVisible { get; set; }
        public bool IsSubdistributorVisible { get; set; }
        public bool IsFarmerVisible { get; set; }
        public bool IsNonAuthenticatedVisible { get; set; }
        public bool IsPublished { get; set; }
        public DateTime? PublishDate { get; set; }

        public int S1Id { get; set; }
        [AllowHtml]
        public string S1Paragraph { get; set; }
        public HttpPostedFileBase S1Image { get; set; }
        public bool IsS1ImageDirty { get; set; }
        public string S1ImageName { get; set; }
        public string S1ImageFooter { get; set; }

        public int S2Id { get; set; }
        [AllowHtml]
        public string S2Paragraph { get; set; }
        public HttpPostedFileBase S2Image { get; set; }
        public bool IsS2ImageDirty { get; set; }
        public string S2ImageName { get; set; }
        public string S2ImageFooter { get; set; }

        public int S3Id { get; set; }
        [AllowHtml]
        public string S3Paragraph { get; set; }
        public HttpPostedFileBase S3Image { get; set; }
        public bool IsS3ImageDirty { get; set; }
        public string S3ImageName { get; set; }
        public string S3ImageFooter { get; set; }

        public int S4Id { get; set; }
        [AllowHtml]
        public string S4Paragraph { get; set; }
        public HttpPostedFileBase S4Image { get; set; }
        public bool IsS4ImageDirty { get; set; }
        public string S4ImageName { get; set; }
        public string S4ImageFooter { get; set; }

        public int S5Id { get; set; }
        [AllowHtml]
        public string S5Paragraph { get; set; }
        public HttpPostedFileBase S5Image { get; set; }
        public bool IsS5ImageDirty { get; set; }
        public string S5ImageName { get; set; }
        public string S5ImageFooter { get; set; }
    }
}