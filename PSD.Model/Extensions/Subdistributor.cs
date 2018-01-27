using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model
{
    public partial class Subdistributor
    {
        #region Attributes
        #endregion

        #region Properties
        //public string SelectItemName { get { return BusinessName + ": " + CommercialNames.FirstOrDefault() != null ? CommercialNames.FirstOrDefault().Name : ""; } }
        public string DisplayName { get { return CommercialNames.FirstOrDefault() != null ? CommercialNames.FirstOrDefault().Name : BusinessName; } }

        //public ContractSubdistributor CurrentContract { get { return Contracts.Where(x => x.Year == PSD.Common.Dates.Today.Year).FirstOrDefault(); } }
        

        #endregion

        #region Constructors
        public static Subdistributor NewEmpty()
        {
            return new Subdistributor()
            {
                Id = -1,
                BusinessName = string.Empty,
                CommercialNames = new List<SubdistributorCommercialName>(),
                IdB = string.Empty,
                WebSite = string.Empty,
                BNAddress = Address.NewEmpty(),
                CropsXMunicipality = new List<SubdistributorCropsXMunicipality>(),
                SubdistributorContacts = new List<SubdistributorContact>(),
                SubdistributorEmployees = new List<SubdistributorEmployee>(),
                BNLegalRepresentative = string.Empty,
                Addresses = new List<AddressesXSubdistributor>(),
                Type = string.Empty                
            };
        }
        #endregion

        #region Methods
        #endregion

        #region Others
        #endregion
    }
}
