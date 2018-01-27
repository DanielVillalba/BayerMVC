using PSD.Model;

namespace PSD.Web.Areas.Content.Models
{
    public class DetailViewModel
    {
        public News DetailNews { get; set; }
        public News PreviousNews { get; set; }
        public News NextNews { get; set; }
    }
}