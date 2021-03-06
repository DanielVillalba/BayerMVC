//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PSD.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class News
    {
        public News()
        {
            this.NewsSections = new HashSet<NewsSection>();
        }
    
        public int Id { get; set; }
        public string URLId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Paragraph { get; set; }
        public string Image { get; set; }
        public string ImageFooter { get; set; }
        public string Author { get; set; }
        public bool IsDistributorVisible { get; set; }
        public bool IsSubdistributorVisible { get; set; }
        public bool IsFarmerVisible { get; set; }
        public bool IsPublished { get; set; }
        public bool IsNonAuthenticatedVisible { get; set; }
        public Nullable<System.DateTime> PublishDate { get; set; }
    
        public virtual ICollection<NewsSection> NewsSections { get; set; }
    }
}
