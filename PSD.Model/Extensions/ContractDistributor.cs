using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model
{
    public partial class ContractDistributor
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        #region Constructors
        public static ContractDistributor NewEmpty()
        {
            return new ContractDistributor()
            {
                Id = -1,
                IdB = string.Empty,
                ContractDate = DateTime.Now,
                Distributor = Distributor.NewEmpty(),
                GRVBayerEmployee = BayerEmployee.NewEmpty(),
                RTVBayerEmployee = BayerEmployee.NewEmpty(),
                Year = DateTime.Now.Year
                
            };
        }
        #endregion

        #region Methods
        #endregion

        #region Others
        #endregion
    }
}
