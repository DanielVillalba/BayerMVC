using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model
{
    public partial class Distributor
    {
        #region Attributes
        #endregion

        #region Properties
        public string SelectItemName { get { return BusinessName + ": " + CommercialName; } }
        public string DisplayName { get { return CommercialName; } }
        #endregion

        #region Constructors
        public static Distributor NewEmpty()
        {
            return new Distributor()
            {
                Id = -1,
                BusinessName = string.Empty,
                CommercialName = string.Empty,
                IdB = string.Empty,
                WebSite = string.Empty,
                Address = Address.NewEmpty(),
                CropsXMunicipality = new List<DistributorCropsXMunicipality>(),
                DistributorContacts = new List<DistributorContact>(),
                DistributorUsers = new List<DistributorEmployee>()
            };
        }
        #endregion

        #region Methods
        #endregion

        #region Others
        #endregion
    }
}
