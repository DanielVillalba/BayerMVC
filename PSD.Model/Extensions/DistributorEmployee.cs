using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model
{
    public partial class DistributorEmployee
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public static DistributorEmployee NewEmpty()
        {
            return new DistributorEmployee()
            {
                Id = -1,
                Distributor = Distributor.NewEmpty(),
                EMail = string.Empty,
                Name = string.Empty,
                LastNameF = string.Empty,
                LastNameM = string.Empty,
                PhoneNumber = string.Empty,
                User = User.NewEmpty(),
            };
        }
        #endregion

        #region Methods
        #endregion

        #region Others
        #endregion
    }
}
