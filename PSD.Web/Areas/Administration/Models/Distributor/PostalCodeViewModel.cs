using PSD.Model;
using System.Collections.Generic;


namespace PSD.Web.Areas.Administration.Models.Distributor
{
    public class PostalCodeViewModel
    {
        public PostalCodeViewModel()
        {
            Address = new Address();
            AvailableColonies = new List<AvailableColoniesViewModel>();
        }
        public Address Address { get; set; }
        public List<AvailableColoniesViewModel> AvailableColonies { get; set; }
        public string Error { get; set; }
        public string StateName { get; set; }
        public string MunicipalityName { get; set; }
    }
}