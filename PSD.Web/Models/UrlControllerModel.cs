using System.Collections.Generic;

namespace PSD.Web.Models
{
    public class UrlControllerModel
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public List<UrlActionModel> Actions { get; set; }

    }
}