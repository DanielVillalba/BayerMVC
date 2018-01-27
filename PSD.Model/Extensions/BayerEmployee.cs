using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model
{
    public partial class BayerEmployee
    {
        #region Attributes
        #endregion

        #region Properties
        public string NameFull { get { return Name + " " + LastNameF + " " + LastNameM; } }
        public string NameDisplay { get { return Name + " " + LastNameF; } }
        #endregion

        #region Constructors
        public static BayerEmployee NewEmpty()
        {
            return new BayerEmployee()
            {
                Id = -1,
                EMail = string.Empty,
                Name = string.Empty,
                LastNameF = string.Empty,
                LastNameM = string.Empty,
                PhoneNumber = string.Empty                
            };
        }
        #endregion

        #region Methods
        #endregion

        #region Others
        #endregion


    }
}
