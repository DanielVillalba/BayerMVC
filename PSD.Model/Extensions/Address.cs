using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model
{
    public partial class Address
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public static Address NewEmpty()
        {
            return new Address()
            {
                Street = string.Empty
                , NumberExt = string.Empty
                , NumberInt = string.Empty
            };
        }
        #endregion

        #region Methods
        public string ToString()
        {
            return Street + " No. "
                + NumberExt
                + ", Int. "
                + NumberInt + ". "
                + (AddressColony != null? 
                ". "
                + AddressColony.Name + ". "
                + AddressColony.AddressPostalCode.AddressMunicipality.Name
                + ". " + AddressColony.AddressPostalCode.AddressMunicipality.State.Name
                + ". CP " + AddressColony.AddressPostalCode.Name
                : "")
                ;
        }
        #endregion

        #region Others
        #endregion
    }
}
