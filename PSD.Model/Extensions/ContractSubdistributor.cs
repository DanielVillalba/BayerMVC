using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSD.Model
{
    public partial class ContractSubdistributor
    {
        #region Attributes
        #endregion

        #region Properties
        public decimal GoalTotalPercentage { get { return (AmountGoalTotal <= 0 ? 0 : (PurchaseTotal / AmountGoalTotal) * 100); } }
        public int GoalTotalPercentageInt { get { return Convert.ToInt32(GoalTotalPercentage); } }
        public string GoalTotalPercentageStr { get { return PSD.Common.Strings.FormatPercentage(GoalTotalPercentage); } }
        
        public decimal GoalTotalS1Percentage { get { return (AmountGoalTotal <= 0 ? 0 : (PurchaseTotalS1 / AmountGoalS1) * 100); } }
        public int GoalTotalS1PercentageInt { get { return Convert.ToInt32(GoalTotalS1Percentage); } }
        public string GoalTotalS1PercentageStr { get { return PSD.Common.Strings.FormatPercentage(GoalTotalS1Percentage); } }

        public decimal GoalTotalS2Percentage { get { return (AmountGoalTotal <= 0 ? 0 : (PurchaseTotalS2 / AmountGoalS2) * 100); } }
        public int GoalTotalS2PercentageInt { get { return Convert.ToInt32(GoalTotalS2Percentage); } }
        public string GoalTotalS2PercentageStr { get { return PSD.Common.Strings.FormatPercentage(GoalTotalS2Percentage); } }
        #endregion

        #region Constructors
        public static ContractSubdistributor NewEmpty()
        {
            return new ContractSubdistributor()
            {
                Id = -1,
                IdB = string.Empty,
                ContractDate = PSD.Common.Dates.Today,
                Subdistributor = Subdistributor.NewEmpty(),
                GRVBayerEmployee = BayerEmployee.NewEmpty(),
                RTVBayerEmployee = BayerEmployee.NewEmpty(),
                Year = PSD.Common.Dates.Today.Year,
                DistributorPurchases = new List<DistributorPurchasesXContractSubdistributor>(),
                RegisteredRegionName = string.Empty,
                RegisteredZoneName = string.Empty
            };
        }
        #endregion

        #region Methods
        #endregion

        #region Others
        #endregion
    }
}
