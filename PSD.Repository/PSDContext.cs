using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PSD.Model;
using System.Data.Entity;

namespace PSD.Repository
{
    public class PSDContext : DbContext
    {
        public PSDContext()
            : base("name=PSDModelContainer")
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Database.Log = this.SendToConsole;
        }

        public void SendToConsole(string message)
        {
            Debug.Write(message);
        }

        public virtual DbSet<AppConfiguration> AppConfigurations { get; set; }

        #region catalogs
        public virtual DbSet<Cat_Crop> Crops { get; set; }
        public virtual DbSet<Cat_DistributorBranchContactArea> DistributorBranchContactAreas { get; set; }
        public virtual DbSet<Cat_DistributorContactArea> DistributorContactAreas { get; set; }
        public virtual DbSet<Cat_SubdistributorContactArea> SubdistributorContactAreas { get; set; }
        public virtual DbSet<Cat_UserRole> UserRoles { get; set; }
        public virtual DbSet<Cat_UserStatus> UserStatuses { get; set; }
        public virtual DbSet<Cat_Zone> Zones { get; set; }
        public virtual DbSet<Cat_ContractDistributorStatus> ContractDistributorStatuses { get; set; }
        public virtual DbSet<Cat_CropCategory> CropCategories { get; set; }

        #endregion

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<AddressColony> AddressColonies { get; set; }
        public virtual DbSet<AddressMunicipality> AddressMunicipalities { get; set; }
        public virtual DbSet<AddressPostalCode> AddressPostalCodes { get; set; }
        public virtual DbSet<AddressState> AddressStates { get; set; }
        public virtual DbSet<AddressesXSubdistributor> AddressesXDistributor { get; set; }

        public virtual DbSet<BayerEmployee> BayerEmployees { get; set; }
        public virtual DbSet<SubdistributorCommercialName> SubdistributorCommercialNames { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<RolesXUser> RolesXUser { get; set; }

        public virtual DbSet<Distributor> Distributors { get; set; }
        public virtual DbSet<DistributorCropsXMunicipality> DistributorCropsXMunicipality { get; set; }
        public virtual DbSet<DistributorBranch> DistributorBranches { get; set; }
        public virtual DbSet<DistributorEmployee> DistributorEmployees  { get; set; }
        public virtual DbSet<DistributorContact> DistributorContacts  { get; set; }
        public virtual DbSet<DistributorBranchContact> DistributorBranchContacts { get; set; }
        public virtual DbSet<ContractDistributor> ContractsDistributor { get; set; }
        public virtual DbSet<ContractSubdistributor> ContractsSubdistributor { get; set; }
        public virtual DbSet<SubdistributorDiscountCoupon> SubdistributorDiscountCoupons { get; set; }
        public virtual DbSet<SubdistributorPromotionCoupon> SubdistributorPromotionCoupons { get; set; }


        public virtual DbSet<MunicipalitiesXEmployee> MunicipalitiesXEmployee { get; set; }

        public virtual DbSet<Subdistributor> Subdistributors { get; set; }
        public virtual DbSet<SubdistributorContact> SubdistributorContacts { get; set; }
        public virtual DbSet<SubdistributorCropsXMunicipality> SubdistributorCropsXMunicipality { get; set; }
        public virtual DbSet<SubdistributorEmployee> SubdistributorEmployees { get; set; }
        public virtual DbSet<DistributorPurchasesXContractSubdistributor> DistributorPurchasesXContractSubdistributors { get; set; }

        #region Content
        public virtual DbSet<ContentLink> ContentLinks { get; set; }
        #endregion
    }
}
