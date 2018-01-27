using System.Collections.Generic;

namespace PSD.Web.Models
{
    public class UrlAreaModel
    {

        public string Id { get; set; }
        public string DisplayName { get; set; }
        public List<UrlControllerModel> Controllers { get; set; }

    }
}