using System;

using PSD.Repository.Core.Repositories;

namespace PSD.Repository.Core
{
    public interface IUnitOfWork : IDisposable
    {
        bool IsDisposed { get; }

        IAppConfigurationRepository AppConfigurations { get; }
        #region Catalogs

        ICatUserRoleRepository UserRoles { get; }
        ICatUserStatusRepository UserStatuses { get; }
        ICatZoneRepository Zones { get; }
        ICatDistributorBranchContactAreaRepository DistributorBranchContactAreas { get; }
        ICatDistributorContactAreaRepository DistributorContactAreas { get; }
        ICatSubdistributorContactAreaRepository SubdistributorContactAreas { get; }
        ICatCropRepository Crops { get; }
        ICatCropCategoryRepository CropCategories { get; }


        ICatContractDistributorStatusRepository ContractDistributorStatuses { get; }
        
        #endregion

        IRolesXUserRepository RolesXUser { get; }
        IUserRepository Users { get; }
        IPersonRepository Persons { get; }
        
        IBayerEmployeeRepository BayerEmployees { get; }
        IMunicipalitiesXEmployeeRepository MunicipalitiesXEmployee { get; }

        IDistributorRepository Distributors { get; }
        IDistributorEmployeeRepository DistributorEmployees { get; }
        IDistributorBranchContactRepository DistributorBranchContacts { get; }
        IDistributorBranchRepository DistributorBranches { get; }
        IDistributorContactRepository DistributorContacts { get; }
        IAddressesXSubdistributorRepository AddressesXSubdistributor { get; }
        IDistributorCropsXMunicipalityRepository DistributorCropsXMunicipality { get; }
        IContractDistributorRepository ContractsDistributor { get; }
        IContractSubdistributorRepository ContractsSubdistributor { get; }
        ISubdistributorDiscountCouponRepository SubdistributorDiscountCoupons { get; }
        ISubdistributorPromotionCouponRepository SubdistributorPromotionCoupons { get; }
        

        ISubdistributorRepository Subdistributors { get; }
        ISubdistributorEmployeeRepository SubdistributorEmployees { get; }
        ISubdistributorContactRepository SubdistributorContacts { get; }
        ISubdistributorCropsXMunicipalityRepository SubdistributorCropsXMunicipality { get; }
        ISubdistributorCommercialNameRepository SubdistributorCommercialNames { get; }
        IDistributorPurchasesXContractSubdistributorRepository DistributorPurchasesXContractSubdistributors { get; }
        
        IAddressRepository Addresses { get; }
        IAddressColonyRepository AddressColonies { get; }
        IAddressPostalCodeRepository AddressPostalCodes { get; }
        IAddressMunicipalityRepository AddressMunicipalities { get; }
        IAddressStateRepository AddressStates { get; }

        #region Content
        IContentLinkRepository ContentLinks { get; }
        #endregion

        int Complete();
    }
}